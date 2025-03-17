using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] RectTransform ecran;

    public void StartTransition(string sceneName)
    {
        ecran.gameObject.SetActive(true); 

        LeanTween.scale(ecran, Vector3.zero, 0f);

        LeanTween.scale(ecran, new Vector3(1, 1, 1), 0.5f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() => {
                LoadScene(sceneName);
            });
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TransitionToScene(string sceneName)
    {
        StartTransition(sceneName);
    }
}
