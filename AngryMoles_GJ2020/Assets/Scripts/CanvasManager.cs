using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [MenuItem("StartGame")]
    public void PlayButton()
    {
        gameObject.SetActive(false);
        //activate Camera Zoom

    }

    [MenuItem("OpenCanvas")]
    public void OpenCanvas()
    {
        gameObject.SetActive(true);
    }

}
