using UnityEngine;
using Mirror;

public class PlayerBase : Building
{
    public GameObject buildingMenu;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Name = "PlayerBase";
        MaxHealth = 500;
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
        Debug.Log("Player base is set up.");
    }

    [Command]
    public void CmdSelect()
    {
        RpcShowBuildingMenu();
    }

    [ClientRpc]
    private void RpcShowBuildingMenu()
    {
        if (buildingMenu != null)
            buildingMenu.SetActive(true);
    }
}