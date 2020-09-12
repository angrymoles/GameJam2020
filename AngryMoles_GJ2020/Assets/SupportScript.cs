using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SupportScript : MonoBehaviour
{
    public Audiomanager audiomanager;
    public GameObject SceneManager;
    // Start is called before the first frame update
    void Start()
    {
        audiomanager = FindObjectOfType<Audiomanager>();
        if (audiomanager != null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
