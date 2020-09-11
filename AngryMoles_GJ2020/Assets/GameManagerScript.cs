using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public float transitionTime = 1f;
     
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }           
        DontDestroyOnLoad(gameObject);
        return;
       
    }

    public void Update()
    {
        ESCKey();
    }
    public void TransitionScene()
    {
        
    }

    public void LoadMenu()
    {
        
        SceneManager.LoadScene("Level0");
    }



    public void FadeToLevel(int levelIndex)
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        var rng = Random.Range(1, 10);
        SceneManager.LoadScene("Level" + rng);
    }
    

       public void ESCKey()
    {
        if (Input.GetKey(KeyCode.Escape))
        {            
            if (SceneManager.GetActiveScene().name != "Level0"&& SceneManager.GetActiveScene().name != "LoadScreen")
            {               
                SceneManager.LoadScene("Level0");
            }
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        levelIndex= Random.Range(1, 10);
        GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }

    public void EXITGAME()
    {
        Debug.Log("IQuit");
        Application.Quit();
    }
    
    //  DontDestroyOnLoad();
}

