using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    private static GameAssets _i;   


    public static GameAssets i { 
        get{
            if(_i==null)   _i=  (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return i;
        }
    }
           
           
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip { 

    public NewSoundManager.Sound sound;
    public AudioClip audioClip;
    }
}

