using UnityEngine;
using Mirror;

public class Farm : Building
{
    [SyncVar] public int FoodProductionRate = 5;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Name = "Farm";
        MaxHealth = 100;
        BuildingCost = 50;
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
        Debug.Log("Farm is now producing food.");
    }

    [Command]
    public void CmdHarvestFood(NetworkIdentity playerIdentity)
    {
        if (!IsConstructed) return;

        if (playerIdentity.TryGetComponent(out NetworkPlayer player))
        {
            player.CmdGatherResources("food", FoodProductionRate);
            RpcNotifyHarvest(player.playerId);
        }
    }

    [ClientRpc]
    private void RpcNotifyHarvest(int playerId)
    {
        Debug.Log($"Player {playerId} harvested {FoodProductionRate} food.");
    }
}