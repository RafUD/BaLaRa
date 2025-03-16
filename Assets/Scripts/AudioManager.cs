using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    [SerializeField] Slider volumeSlider;
    public static AudioManager instance;
    public JeuFinni jeu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;   
            s.source.loop = s.loop; 
        }
    }

    private void Start()
    {


        if (SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            Play("JeuMenu");
        }
        if (SceneManager.GetActiveScene().name.Equals("Jeu Finni"))
        {
            Play("JeuFinni");   
        }
        if (SceneManager.GetActiveScene().name.StartsWith("Niveau") || SceneManager.GetActiveScene().name.Equals("Tutoriel"))
        {
            Play("JeuAmbience");
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " n'existe pas.");
            return;
        }

        s.source.Play();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }


}
