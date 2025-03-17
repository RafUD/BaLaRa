using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityManager : NetworkBehaviour
{
    public static EntityManager Instance { get; private set; }

    public readonly SyncList<Soldier> Soldiers = new SyncList<Soldier>();
    public readonly SyncList<Building> Buildings = new SyncList<Building>();

    private Dictionary<uint, Soldier> soldierLookup = new Dictionary<uint, Soldier>();
    private Dictionary<uint, Building> buildingLookup = new Dictionary<uint, Building>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Server]
    public void AddSoldier(Soldier soldier)
    {
        if (soldier == null || soldierLookup.ContainsKey(soldier.netId)) return;

        Soldiers.Add(soldier);
        soldierLookup[soldier.netId] = soldier;
        NetworkServer.Spawn(soldier.gameObject);
    }

    [Server]
    public void RemoveSoldier(Soldier soldier)
    {
        if (soldier == null || !Soldiers.Contains(soldier)) return;

        Soldiers.Remove(soldier);
        soldierLookup.Remove(soldier.netId);
        NetworkServer.Destroy(soldier.gameObject);
    }

    [Server]
    public void AddBuilding(Building building)
    {
        if (building == null || buildingLookup.ContainsKey(building.netId)) return;

        Buildings.Add(building);
        buildingLookup[building.netId] = building;
        NetworkServer.Spawn(building.gameObject);
    }

    [Server]
    public void RemoveBuilding(Building building)
    {
        if (building == null || !Buildings.Contains(building)) return;

        Buildings.Remove(building);
        buildingLookup.Remove(building.netId);
        NetworkServer.Destroy(building.gameObject);
    }

    public Soldier GetSoldierById(uint netId)
    {
        return soldierLookup.TryGetValue(netId, out Soldier soldier) ? soldier : null;
    }

    public Building GetBuildingById(uint netId)
    {
        return buildingLookup.TryGetValue(netId, out Building building) ? building : null;
    }

    public List<T> GetBuildingsByType<T>() where T : Building
    {
        List<T> result = new List<T>();
        foreach (var building in Buildings)
        {
            if (building is T typedBuilding)
            {
                result.Add(typedBuilding);
            }
        }
        return result;
    }

    private void Update()
    {
        if (!isServer) return;

        foreach (var soldier in Soldiers)
        {
            soldier.UpdateEntity();
        }

        foreach (var building in Buildings)
        {
            building.UpdateBuilding();
        }
    }
}