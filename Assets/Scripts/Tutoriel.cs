using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutoriel : MonoBehaviour
{
    private string previousScene;

    void Start()
    {
        if (PlayerPrefs.HasKey("PreviousScene"))
        {
            previousScene = PlayerPrefs.GetString("PreviousScene");
        }
        else
        {
            previousScene = "Menu"; 
        }

        Time.timeScale = 0f;
    }

    public void Retourner()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(previousScene);
    }
}
