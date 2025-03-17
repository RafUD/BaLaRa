using UnityEngine;
using Mirror;

public class NetworkManagerRTS : NetworkManager
{
    public GameObject playerBasePrefab;
    public GameObject soldierPrefab;

    private Vector3[] basePositions = { new Vector3(-11, -5, 0), new Vector3(15, 5, 0) };
    private Vector3[] soldierPositions = { new Vector3(-9, -5, 0), new Vector3(13, 4, 0) };

    private int readyPlayers = 0;
    private bool gameStarted = false;

    [System.Obsolete]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        readyPlayers++;

        Debug.Log($"Player joined. Current players: {readyPlayers}");

        // Only start the game if both players are connected
        if (readyPlayers == 2 && !gameStarted)
        {
            gameStarted = true;
            FindObjectOfType<MainMenu>().OnSecondPlayerConnected();
        }
    }

    [System.Obsolete]
    public void StartGame()
    {
        Debug.Log("Starting game, spawning player bases and soldiers...");

        for (int i = 0; i < readyPlayers; i++)
        {
            NetworkConnectionToClient conn = NetworkServer.connections[i];

            GameObject playerBase = Instantiate(playerBasePrefab, basePositions[i], Quaternion.identity);
            NetworkServer.Spawn(playerBase, conn);

            GameObject soldier = Instantiate(soldierPrefab, soldierPositions[i], Quaternion.identity);
            NetworkServer.Spawn(soldier, conn);
        }
    }

    public void CheckIfBothPlayersAreReady()
    {
        readyPlayers++;

        if (readyPlayers == 2 && !gameStarted)
        {
            gameStarted = true;
            StartGame();
        }
    }
}
