using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void OnReplayClicked()
    {
       
        //GameManagerScript.Get().OnFadeComplete();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
