using UnityEngine;
using Mirror;

public class Barracks : Building
{
    [SyncVar] public int TrainingCapacity = 5;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Name = "Barracks";
        MaxHealth = 200;
        BuildingCost = 150;
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
        Debug.Log($"{Name} has been constructed and is ready.");
    }

    [Command]
    public void CmdTrainUnit()
    {
        if (!IsConstructed) return;

        RpcNotifyTraining();
    }

    [ClientRpc]
    private void RpcNotifyTraining()
    {
        Debug.Log("A new soldier is being trained.");
    }
}