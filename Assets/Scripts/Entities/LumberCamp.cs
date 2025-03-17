using UnityEngine;
using Mirror;

public class LumberCamp : Building
{
    [SyncVar] public int WoodStorage = 200;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Name = "Lumber Camp";
        MaxHealth = 150;
        BuildingCost = 75;
        CurrentHealth = MaxHealth;
    }

    [Command]
    public override void CmdConstruct()
    {
        base.CmdConstruct();
        RpcNotifyConstruction();
    }

    [ClientRpc]
    private void RpcNotifyConstruction()
    {
        Debug.Log("Lumber Camp is ready to store wood.");
    }

    [Command]
    public void CmdStoreWood(NetworkIdentity playerIdentity, int amount)
    {
        if (!IsConstructed) return;

        WoodStorage += amount;

        if (playerIdentity.TryGetComponent(out Player player))
        {
            player.CmdGatherResources("wood", amount);
            RpcNotifyWoodStorage(player.playerId, amount);
        }
    }

    [ClientRpc]
    private void RpcNotifyWoodStorage(int playerId, int amount)
    {
        Debug.Log($"Player {playerId} stored {amount} wood. Total stored: {WoodStorage}");
    }
}