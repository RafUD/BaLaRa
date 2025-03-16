using UnityEngine;
using Mirror;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [ClientRpc]
    public void RpcPlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }
}
