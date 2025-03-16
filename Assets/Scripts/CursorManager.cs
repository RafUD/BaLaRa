using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor;
    public Vector2 cursorHotspot = Vector2.zero;
    public AudioClip clickSound;

    public AudioSource audioSource;
    
    void Start()
    {
        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            PlayClickSound();
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
