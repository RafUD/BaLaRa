using UnityEngine;
using Mirror;

public class NetworkManagerRTS : NetworkManager
{
    public GameObject playerBasePrefab;
    public GameObject soldierPrefab;

    private Vector3[] basePositions = { new Vector3(-11, -5, 0), new Vector3(15, 5, 0) };
    private Vector3[] soldierPositions = { new Vector3(-9, -5, 0), new Vector3(13, 4, 0) };

    private int readyPlayers = 0;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        int playerIndex = numPlayers - 1;

        GameObject playerBase = Instantiate(playerBasePrefab, basePositions[playerIndex], Quaternion.identity);
        PlayerBase baseComponent = playerBase.GetComponent<PlayerBase>();
        baseComponent.OwnerId = playerIndex;
        NetworkServer.Spawn(playerBase, conn);

        GameObject soldier = Instantiate(soldierPrefab, soldierPositions[playerIndex], Quaternion.identity);
        Soldier soldierComponent = soldier.GetComponent<Soldier>();
        soldierComponent.ownerId = playerIndex;
        NetworkServer.Spawn(soldier, conn);
    }

    public void CheckIfBothPlayersAreReady()
    {
        readyPlayers++;

        if (readyPlayers == 2)
        {
            Debug.Log("Both players ready, starting game...");
            ServerChangeScene(FindObjectOfType<MainMenu>().selectedLevel);
        }
    }
}
