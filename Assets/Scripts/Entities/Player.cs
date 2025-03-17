using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class Player : NetworkBehaviour
{
    [SyncVar] public int playerId;
    [SyncVar] public string playerName = "Player";

    [SyncVar(hook = nameof(OnWoodChanged))] public int wood = 300;
    [SyncVar(hook = nameof(OnFoodChanged))] public int food = 100;

    public List<Soldier> soldiers = new List<Soldier>();
    public List<Building> buildings = new List<Building>();

    public override void OnStartLocalPlayer()
    {
        GameManager.Instance.RegisterPlayer(this);
        UIManager.Instance.UpdateResources(wood, food);
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

    [Command]
    public void CmdSpawnSoldier(Vector2 spawnPosition)
    {
        if (food < 10) return;

        food -= 10;

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
    public void CmdConstructBuilding(Vector2 buildPosition)
    {
        if (wood < 50) return;

        wood -= 50;

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