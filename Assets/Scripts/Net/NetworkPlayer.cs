using UnityEngine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public int playerId;
    [SyncVar] public string playerName;
    [SyncVar] public bool isReady = false;

    public static NetworkPlayer LocalPlayer { get; private set; }
    public Camera playerCamera; // Assign this in the Player prefab

    public override void OnStartLocalPlayer()
    {
        if (LocalPlayer != null && LocalPlayer != this)
        {
            Debug.LogWarning($"[OnStartLocalPlayer] Duplicate LocalPlayer detected! Skipping assignment for {gameObject.name} with NetID: {netId}");
            return;
        }

        LocalPlayer = this;
        Debug.Log($"[OnStartLocalPlayer] Triggered for {gameObject.name} with NetID: {netId}");

        // Activate the camera for the local player
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            Debug.Log($"[OnStartLocalPlayer] Camera enabled for local player: {gameObject.name}");
        }
        else
        {
            Debug.LogError("[OnStartLocalPlayer] Player camera is missing in the prefab!");
        }
    }
}
