using UnityEngine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public int playerId;
    [SyncVar] public string playerName;
    [SyncVar] public bool isReady = false;

    public static NetworkPlayer LocalPlayer { get; private set; }

    public override void OnStartLocalPlayer()
    {
        LocalPlayer = this;
        CmdSetPlayerInfo($"Player {connectionToClient.connectionId}");
    }

    [Command]
    void CmdSetPlayerInfo(string newName)
    {
        playerName = newName;
    }

    [Command]
    public void CmdSetReady()
    {
        isReady = true;
        FindObjectOfType<NetworkManagerRTS>().CheckIfBothPlayersAreReady();
    }
}
