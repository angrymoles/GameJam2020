using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScribt : MonoBehaviour
{
    public Audiomanager audio;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audio.PlayOneTimeSound("EnemyFire");
        }
        
    }
}
