using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //public static GameManagerScript instance;
    public float transitionTime = 1f;
    public int EscMessageNumber=-1;
    
    // Start is called before the first frame update
    void Start()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(this);
        //}           
        //DontDestroyOnLoad(gameObject);
        //return;
       
    }

    public void Update()
    {
        ESCKey();
    }
    public void LoadRandomScene()
    {
        var rng= Random.Range(1, 10);
        StartCoroutine(LoadLevel(rng));
    }

    public void FadeToLevel(int levelIndex)
    {
       GetComponent<Animator>().SetTrigger("FadeOut");
    }

    //public void OnFadeComplete()
    //{
    //    var rng = Random.Range(1, 10);
    //    SceneManager.LoadScene("Level" + rng);
    //}
    

   public void ESCKey()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(LoadLevel(0));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {

        if (levelIndex == 0) { }
        else if (levelIndex == 11) { }
        else
        {
            levelIndex = Random.Range(1, 10);
        }
       
        GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);   

    }

    public void LoadDeathScreen()
    {
        StartCoroutine(LoadLevel(11));
    }

    public void EXITGAME()
    {
        Application.Quit();
    }
    
    //  DontDestroyOnLoad();
}

