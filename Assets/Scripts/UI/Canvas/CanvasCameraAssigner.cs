using UnityEngine;

public class CanvasCameraAssigner : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        if (canvas.worldCamera == null)
            canvas.worldCamera = Camera.main;
    }
}
