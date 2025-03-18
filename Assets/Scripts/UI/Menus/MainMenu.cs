using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels & Buttons")]
    public GameObject menuPanel;
    public GameObject joinGamePanel;
    public GameObject optionsMenu;
    public GameObject waitingForPlayerPanel;
    public GameObject joinLANButton;
    public TextMeshProUGUI statusText;

    private CustomNetworkDiscovery networkDiscovery;
    private ServerResponse discoveredServer;
    private NetworkManagerRTS networkManagerRTS;
    public string selectedLevel;

    public static MainMenu Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [System.Obsolete]
    void Start()
    {
        networkDiscovery = FindObjectOfType<CustomNetworkDiscovery>();
        networkManagerRTS = FindObjectOfType<NetworkManagerRTS>();
        menuPanel.SetActive(true);
        joinGamePanel.SetActive(false);
        optionsMenu.SetActive(false);
        waitingForPlayerPanel.SetActive(false);
        joinLANButton.SetActive(false);
    }

    public void Niveau1()
    {
        selectedLevel = "Niveau 1";
        OpenWaitingRoom();
    }

    public void Niveau2()
    {
        selectedLevel = "Niveau 2";
        OpenWaitingRoom();
    }

    public void Niveau3()
    {
        selectedLevel = "Niveau 3";
        OpenWaitingRoom();
    }

    public void SortirJeuMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenJoinGamePanel()
    {
        menuPanel.SetActive(false);
        joinGamePanel.SetActive(true);
        joinLANButton.SetActive(false);
        statusText.text = "Searching for LAN games...";
        statusText.color = Color.white;

        if (networkDiscovery != null)
        {
            Debug.Log("Starting LAN game discovery...");
            networkDiscovery.StartDiscovery();
        }
        else
        {
            Debug.LogError("CustomNetworkDiscovery not assigned in MainMenu!");
        }
    }


    public void BackToMainMenuFromJoin()
    {
        joinGamePanel.SetActive(false);
        menuPanel.SetActive(true);
        networkDiscovery.StopDiscovery();
    }

    public void OpenOptionsMenu()
    {
        menuPanel.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackFromOptions()
    {
        optionsMenu.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenWaitingRoom()
    {
        menuPanel.SetActive(false);
        waitingForPlayerPanel.SetActive(true);
        networkManagerRTS.StartHost();
        networkDiscovery.AdvertiseServer();
    }

    public void BackToMainMenuFromWaiting()
    {
        waitingForPlayerPanel.SetActive(false);
        menuPanel.SetActive(true);

        if (networkDiscovery != null)
        {
            networkDiscovery.StopDiscovery();
        }

        networkManagerRTS.StopHost();
    }


    public void JoinDiscoveredServer()
    {
        if (discoveredServer.uri != null)
        {
            string localUri = discoveredServer.uri.ToString().Replace(discoveredServer.ipAddress, "127.0.0.1");
            Debug.Log($"[LAN] Connecting to {localUri}");

            NetworkManager.singleton.StartClient(new Uri(localUri));
        }
        else
        {
            Debug.LogError("[LAN] No valid server URI found.");
        }
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        if (info.currentPlayers == 1)
        {
            discoveredServer = info;
            statusText.text = $"Game Found at: {info.ipAddress}:{info.port}";
            statusText.color = Color.green;
            joinLANButton.SetActive(true);

            Debug.Log($"[LAN Discovery] Found Server at: {info.ipAddress}:{info.port}");
        }
    }

    public void OnSecondPlayerConnected()
    {
        Debug.Log("[MainMenu] Second player connected! Changing scene...");

        if (networkManagerRTS == null)
        {
            Debug.LogError("[MainMenu] NetworkManagerRTS is missing!");
            return;
        }

        if (string.IsNullOrEmpty(selectedLevel))
        {
            Debug.LogError("[MainMenu] No level selected! Cannot change scene.");
            return;
        }

        networkManagerRTS.ServerChangeScene(selectedLevel);
    }
}