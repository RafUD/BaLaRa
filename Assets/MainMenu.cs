using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Niveau1()
    {
        SceneManager.LoadScene("Niveau 1");
    }
    public void Niveau2()
    {
        SceneManager.LoadScene("Niveau 2");
    }
    public void Niveau3()
    {
        SceneManager.LoadScene("Niveau 3");
    }

    public void SortirJeuMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
