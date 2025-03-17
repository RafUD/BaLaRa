using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerRTS : NetworkManager
{
    public GameObject playerBasePrefab;
    public GameObject soldierPrefab;
    public GameObject hudPrefab; // Assign HUD prefab here

    public Vector3 playerOneBasePosition;
    public Vector3 playerTwoBasePosition;
    public Vector3 playerOneSoldierPosition;
    public Vector3 playerTwoSoldierPosition;

    private int readyPlayers = 0;
    private bool gameStarted = false;

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
            FindObjectOfType<MainMenu>()?.OnSecondPlayerConnected();
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName.StartsWith("Niveau") && readyPlayers == 2)
        {
            Debug.Log("Both players are ready, starting the game...");
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
                Debug.LogWarning($"Skipping player {conn.connectionId}: NetworkIdentity is null or destroyed.");
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

    public void CheckIfBothPlayersAreReady()
    {
        readyPlayers++;

        if (readyPlayers == 2 && !gameStarted)
        {
            gameStarted = true;
            Debug.Log("Both players ready, starting game...");
            StartGame();
        }
    }
}
