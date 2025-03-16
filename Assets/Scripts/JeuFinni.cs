using UnityEngine;
using UnityEngine.SceneManagement;

public class JeuFinni : MonoBehaviour
{
    public bool jeuGagne;
    public GameObject jeuGagneEcran;
    public GameObject jeuPerduEcran;
    private string currentScene;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        currentScene = SceneManager.GetActiveScene().name;
        

        if (jeuGagne)
        {
            jeuGagneEcran.SetActive(true);
        }
        else
        {
            jeuPerduEcran.SetActive(true);
        }
    }
    
    public void RetournerMenu(){
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
