using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class Audiomanager : MonoBehaviour
{
    public Sound[] sounds;
    public GameObject volumeSlider;
    // Start is called before the first frame update

    public static Audiomanager instance;   

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }

    }
    
    public void PlayOneTimeSound(string name)
    {       
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
        }
        s.source.Play();
    }
    public void AdjustVolume()
    {
        AudioListener.volume = volumeSlider.GetComponent<Slider>().value;
    }

    
}
