using UnityEngine;

public class CanvasPersist : MonoBehaviour
{
    private static CanvasPersist instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
