using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.PlayerLoop;

public class CanvasManager : MonoBehaviour
{
    
    public Audiomanager audioManager;  

    public void PlayButton()
    {
        gameObject.SetActive(false);
        //activate Camera Zoom

    }
   

    public void OpenCanvas()
    {
        gameObject.SetActive(true);
    }

    public void SetVolumeBySlider()
    {
       // audioManager.AdjustVolume();
    }

}
