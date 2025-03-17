using UnityEngine;
using Mirror;

public class PauseMenu : NetworkBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseIcon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isLocalPlayer)
        {
            CmdTogglePause();
        }
    }

    [Command]
    void CmdTogglePause()
    {
        RpcTogglePause();
    }

    [ClientRpc]
    void RpcTogglePause()
    {
        GameIsPaused = !GameIsPaused;
        Time.timeScale = GameIsPaused ? 0f : 1f;
        pauseMenuUI.SetActive(GameIsPaused);
        pauseIcon.SetActive(!GameIsPaused);
    }
}
