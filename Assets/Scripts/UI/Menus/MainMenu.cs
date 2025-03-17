using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

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

        networkDiscovery.StartDiscovery();
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
            networkManagerRTS.StartClient(discoveredServer.uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        if (info.currentPlayers == 1)
        {
            discoveredServer = info;
            statusText.text = $"Game Found at: {info.ipAddress}:{info.port}";
            statusText.color = Color.green;
            joinLANButton.SetActive(true);
        }
    }
    
    public void OnSecondPlayerConnected()
    {
        networkManagerRTS.ServerChangeScene(selectedLevel);
    }

}