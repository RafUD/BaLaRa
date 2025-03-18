using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public int playerId;
    [SyncVar] public string playerName = "Player";

    [SyncVar(hook = nameof(OnWoodChanged))] public int wood = 300;
    [SyncVar(hook = nameof(OnFoodChanged))] public int food = 100;

    public List<Soldier> soldiers = new List<Soldier>();
    public List<Building> buildings = new List<Building>();

    public static NetworkPlayer LocalPlayer { get; private set; }
    public Camera playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            Debug.Log($"[NetworkPlayer] Start coroutine for NetworkPlayer - notifying server after scene loads");
            StartCoroutine(NotifyServerAfterSceneLoads());
        }
    }

    private IEnumerator NotifyServerAfterSceneLoads()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null; // Wait for scene to be fully loaded
        }

        yield return new WaitForSeconds(1f); // Give extra time for network to stabilize

        Debug.Log($"[NetworkPlayer] Scene loaded for player {connectionToClient.connectionId}. Notifying server...");
        CmdNotifyServerPlayerLoaded();
    }

    [Command]
    private void CmdNotifyServerPlayerLoaded()
    {
        Debug.Log($"[NetworkPlayer] CmdNotifyServerPlayerLoaded() called for player {connectionToClient.connectionId}");

        if (NetworkServer.active)
        {
            ((NetworkManagerRTS)NetworkManager.singleton).PlayerFinishedLoading(connectionToClient);
        }
    }


    public override void OnStartLocalPlayer()
    {
        if (LocalPlayer != null && LocalPlayer != this)
        {
            Debug.LogWarning($"[OnStartLocalPlayer] Duplicate LocalPlayer detected! Skipping assignment for {gameObject.name} with NetID: {netId}");
            return;
        }

        LocalPlayer = this;
        Debug.Log($"[OnStartLocalPlayer] Local Player Initialized: {gameObject.name}");

        // Register the player in GameManager
        GameManager.Instance?.RegisterPlayer(this);

        // Update UI resources
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateResources(wood, food);
            UIManager.Instance.InitializeHUD(); // ✅ Initialize HUD after resources are set
        }
        else
        {
            Debug.LogError("[OnStartLocalPlayer] UIManager instance is missing, HUD cannot be created!");
        }

        // Activate Camera for the Local Player
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            Debug.Log("[OnStartLocalPlayer] Camera enabled for local player.");
        }
        else
        {
            Debug.LogError("[OnStartLocalPlayer] Player camera is missing in the prefab!");
        }
    }

    private void OnWoodChanged(int oldWood, int newWood)
    {
        if (isLocalPlayer)
            UIManager.Instance.UpdateResources(newWood, food);
    }

    private void OnFoodChanged(int oldFood, int newFood)
    {
        if (isLocalPlayer)
            UIManager.Instance.UpdateResources(wood, newFood);
    }

    // COMMANDS & RPCs

    [Command]
    public void CmdSpawnSoldier(Vector2 spawnPosition)
    {
        if (wood < 10) return;

        wood -= 10;

        GameObject soldierInstance = Instantiate(NetworkManager.singleton.spawnPrefabs[0], spawnPosition, Quaternion.identity);
        Soldier newSoldier = soldierInstance.GetComponent<Soldier>();
        newSoldier.ownerId = playerId;

        EntityManager.Instance.AddSoldier(newSoldier);
        soldiers.Add(newSoldier);
        NetworkServer.Spawn(soldierInstance, connectionToClient);
        RpcNotifySpawnSoldier(playerId, spawnPosition);
    }

    [ClientRpc]
    private void RpcNotifySpawnSoldier(int playerId, Vector2 position)
    {
        Debug.Log($"Player {playerId} spawned a soldier at {position}");
    }

    [Command]
    public void CmdConstructBuilding(Vector2 buildPosition, int buildingCost)
    {
        if (wood < buildingCost) return;

        wood -= buildingCost;

        GameObject buildingInstance = Instantiate(NetworkManager.singleton.spawnPrefabs[1], buildPosition, Quaternion.identity);
        Building newBuilding = buildingInstance.GetComponent<Building>();
        newBuilding.OwnerId = playerId;

        EntityManager.Instance.AddBuilding(newBuilding);
        buildings.Add(newBuilding);
        NetworkServer.Spawn(buildingInstance, connectionToClient);
        RpcNotifyConstructBuilding(playerId, buildPosition);
    }

    [ClientRpc]
    private void RpcNotifyConstructBuilding(int playerId, Vector2 position)
    {
        Debug.Log($"Player {playerId} constructed a building at {position}");
    }


    [Command]
    public void CmdGatherResources(string resourceType, int amount)
    {
        switch (resourceType.ToLower())
        {
            case "wood":
                wood += amount;
                break;
            case "food":
                food += amount;
                break;
        }
        RpcUpdateResources(playerId, wood, food);
    }

    [ClientRpc]
    private void RpcUpdateResources(int playerId, int updatedWood, int updatedFood)
    {
        Debug.Log($"Player {playerId} now has: Wood: {updatedWood}, Food: {updatedFood}");
    }

    [Command]
    public void CmdRemoveSoldier(NetworkIdentity soldierIdentity)
    {
        if (soldierIdentity.TryGetComponent(out Soldier soldier))
        {
            soldiers.Remove(soldier);
            EntityManager.Instance.RemoveSoldier(soldier);
            NetworkServer.Destroy(soldierIdentity.gameObject);
        }
    }

    [Command]
    public void CmdRemoveBuilding(NetworkIdentity buildingIdentity)
    {
        if (buildingIdentity.TryGetComponent(out Building building))
        {
            buildings.Remove(building);
            EntityManager.Instance.RemoveBuilding(building);
            NetworkServer.Destroy(buildingIdentity.gameObject);
        }
    }
}
