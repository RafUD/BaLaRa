using UnityEngine;
using Mirror;
using System.Collections;

public abstract class Building : NetworkBehaviour
{
    [SyncVar] public int Id;
    [SyncVar] public int OwnerId;
    [SyncVar] public string Name;
    [SyncVar] public float CurrentHealth;
    [SyncVar] public float MaxHealth;
    [SyncVar] public int BuildingCost;
    [SyncVar] public bool IsConstructed;

    private Coroutine repairCoroutine;
    private float lastDamageTime;

    private const float REPAIR_DELAY = 10f;
    private const float REPAIR_RATE = 1f;

    public override void OnStartServer()
    {
        CurrentHealth = MaxHealth;
    }

    [Command]
    public virtual void CmdConstruct()
    {
        IsConstructed = true;
        RpcNotifyConstruction();
    }

    [ClientRpc]
    protected void RpcNotifyConstruction()
    {
        Debug.Log($"{Name} has been constructed.");
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        if (!IsConstructed || CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        RpcUpdateHealth(CurrentHealth);

        if (CurrentHealth <= 0)
            DestroyBuilding();
        else
            RestartRepairTimer();
    }

    [ClientRpc]
    private void RpcUpdateHealth(float newHealth)
    {
        CurrentHealth = newHealth;
    }

    private IEnumerator RepairCoroutine()
    {
        yield return new WaitForSeconds(10f);

        while (CurrentHealth < MaxHealth)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + 1f, MaxHealth);
            RpcUpdateHealth(CurrentHealth);
            yield return new WaitForSeconds(1f);
        }
    }

    [Server]
    protected virtual void DestroyBuilding()
    {
        RpcDestroyBuilding();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcDestroyBuilding()
    {
        Debug.Log($"{Name} has been destroyed.");
    }

    private void RestartRepairTimer()
    {
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);

        repairCoroutine = StartCoroutine(RepairRoutine());
    }

    private IEnumerator RepairRoutine()
    {
        yield return new WaitForSeconds(10f);

        while (CurrentHealth < MaxHealth)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + 1f, MaxHealth);
            RpcUpdateHealth(CurrentHealth);
            yield return new WaitForSeconds(1f);
        }
    }

    [Command]
    public void CmdSelect()
    {
        RpcSelect();
    }

    [ClientRpc]
    public virtual void RpcSelect()
    {
        Debug.Log($"{Name} selected.");
    }

    public virtual void UpdateBuilding()
    {
        if (CurrentHealth <= 0)
            DestroyBuilding();
    }

}
