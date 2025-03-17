using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Player> players = new List<Player>();
    public bool gameIsRunning;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameIsRunning = true;
    }

    public void RegisterPlayer(Player player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void RemovePlayer(Player player)
    {
        if (players.Contains(player))
            players.Remove(player);

        if (players.Count == 1)
            EndGame();
    }

    public void EndGame()
    {
        gameIsRunning = false;
    }
}
