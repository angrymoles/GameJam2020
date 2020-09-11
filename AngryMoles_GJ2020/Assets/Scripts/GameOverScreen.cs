using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void OnReplayClicked()
    {
        var gameManagerObj = GameObject.FindGameObjectWithTag("GameManager");
        if ( gameManagerObj != null)
        {
            gameManagerObj.GetComponent<GameManagerScript>().LoadDeathScreen();
        }
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
