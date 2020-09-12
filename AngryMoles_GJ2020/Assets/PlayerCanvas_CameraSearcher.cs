using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas_CameraSearcher : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<Camera>();
        GetComponent<Canvas>().worldCamera = cam;
    }

}
