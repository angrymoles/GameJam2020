using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NewSoundManager 
{
    public enum Sound
    {
        Ui_HoverButton,
        Ui_ClickButton,

        CandleShutting,

        Theme_Menu,
        Theme_Battle,
        Theme_Shadow,
        Theme_GameOver,
        
        Hit_Shield,
        Hit_Player,
        Hit_Enemy,
        Hit_Wall,

        Surprise_Ghost,

        Lantern_Close,
        Lanter_Open,

        Enemy_Walking,

        Player_Hurt,
      

        Shadow_Heartbeat,
        Shadow_Transformation,


        

        Dead_Player,
        Dead_Enemy,

        



        
    }


    private static Dictionary<Sound, float> soundTimerDictrionary;


    public static void PlaySound(Sound sounds)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sounds));
    }

    //private static bool CanPlaySound(Sound sound)
    //{
    //    switch (sound)
    //    {
    //        default:
    //            return true;
    //        case Sound.Theme_Menu:
    //            if (soundTimerDictrionary.ContainsKey(sound))
    //            {
    //                float lastTimePlayer = soundTimerDictrionary[sound];
    //                float playerMoveTimerMax = 0.0f;

    //            }
    //            break;
    //    }
    //}

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }

        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
