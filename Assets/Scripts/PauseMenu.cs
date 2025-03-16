using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject pauseIcon;
    private string currentScene; 


    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseIcon.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;   
    }

    public void Pause()
    {
        pauseIcon.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        PlayerPrefs.SetString("PreviousScene", currentScene);
        Resume();

        SceneManager.LoadScene("Menu");
    }

    public void LoadTutoriel()
    {
        PlayerPrefs.SetString("PreviousScene", currentScene);

        SceneManager.LoadScene("Tutoriel");
    }

    public void ResumeFromTutoriel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(currentScene);
    }
}
