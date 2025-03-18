using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerRTS : NetworkManager
{
    public GameObject playerBasePrefab;
    public GameObject soldierPrefab;

    public Vector3 playerOneBasePosition;
    public Vector3 playerTwoBasePosition;
    public Vector3 playerOneSoldierPosition;
    public Vector3 playerTwoSoldierPosition;

    private int readyPlayers = 0;
    private bool gameStarted = false;

    [System.Obsolete]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            Debug.LogWarning($"[OnServerAddPlayer] Skipping duplicate player creation for connection {conn.connectionId}");
            return;
        }

        Debug.Log($"[OnServerAddPlayer] Adding player for connection {conn.connectionId}");

        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        Debug.Log($"[OnServerAddPlayer] Player joined. Current players: {NetworkServer.connections.Count}");
        
        if (NetworkServer.connections.Count == 2 && !gameStarted)
        {
            gameStarted = true;
            Debug.Log("[NetworkManagerRTS] Second player connected. Starting game...");
            FindObjectOfType<MainMenu>()?.OnSecondPlayerConnected();
        }
    }


    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName.StartsWith("Niveau"))
        {
            Debug.Log("Level scene loaded. Waiting for players to confirm loading...");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogError("[NetworkManagerRTS] Cannot change to an empty scene name.");
            return;
        }

        Debug.Log($"[NetworkManagerRTS] Changing scene to: {newSceneName}");

        base.ServerChangeScene(newSceneName);
    }


    [Server]
    public void PlayerFinishedLoading(NetworkConnectionToClient conn)
    {
        readyPlayers++;

        Debug.Log($"[NetworkManagerRTS] Player {conn.connectionId} finished loading. Total ready players: {readyPlayers}");

        if (readyPlayers == 2 && !gameStarted)
        {
            gameStarted = true;
            Debug.Log("[NetworkManagerRTS] Both players are ready! Starting the game...");
            Invoke(nameof(StartGame), 1.0f);
        }
    }

    private void StartGame()
    {
        Debug.Log("Spawning player bases, soldiers...");

        int playerIndex = 0;

        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity == null)
            {
                Debug.LogWarning($"Skipping player {conn.connectionId}: NetworkIdentity is null.");
                continue;
            }

            Vector3 basePosition = (playerIndex == 0) ? playerOneBasePosition : playerTwoBasePosition;
            Vector3 soldierPosition = (playerIndex == 0) ? playerOneSoldierPosition : playerTwoSoldierPosition;

            GameObject playerBase = Instantiate(playerBasePrefab, basePosition, Quaternion.identity);
            NetworkServer.Spawn(playerBase, conn);
            Debug.Log($"Spawned player base for {conn.connectionId} at {basePosition}");

            GameObject soldier = Instantiate(soldierPrefab, soldierPosition, Quaternion.identity);
            NetworkServer.Spawn(soldier, conn);
            Debug.Log($"Spawned soldier for {conn.connectionId} at {soldierPosition}");

            playerIndex++;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Player disconnected: {conn.connectionId}");

        if (conn.identity != null)
        {
            NetworkServer.Destroy(conn.identity.gameObject);
            Debug.Log($"Destroyed player object for connection {conn.connectionId}");
        }

        base.OnServerDisconnect(conn);
    }
}
