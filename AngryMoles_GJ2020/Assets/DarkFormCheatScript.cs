using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DarkFormCheatScript : MonoBehaviour
{
    public GameObject darkform;
    public GameObject Lamp;
    public GameObject PlayerAnimation;
    

    private void Start()
    {
        darkform.SetActive(false);
       
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            darkform.SetActive(true);
            Lamp lamp = Lamp.GetComponent<Lamp>();
            if ( lamp != null )
            {
                lamp.ActivateShadow();
            }
            Lamp.SetActive(false);
            PlayerAnimation.GetComponent<SpriteRenderer>().enabled = false;
           

            
           
        }
        if (Input.GetMouseButtonUp(1))
        {
            darkform.SetActive(false);
            Lamp.SetActive(true);
            Lamp lamp = Lamp.GetComponent<Lamp>();
            if (lamp != null)
            {
                lamp.DeactivateShadow();
            }
            PlayerAnimation.GetComponent<SpriteRenderer>().enabled = true;

        }

    }
}
