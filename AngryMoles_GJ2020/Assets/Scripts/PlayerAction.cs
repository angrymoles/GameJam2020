using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public enum PLAYER_STATE
    {
        E_NORMAL,
        E_SHADOW,
        E_SHIELD,
        E_DEAD,
    }

    public Lamp lamp;
    public PolygonCollider2D shieldCollider;
    public PolygonCollider2D bodyCollider;
    //private PLAYER_STATE playerState;
    private bool shadowActive = false;
    private bool shieldActive = false;    
    // Start is called before the first frame update
    void Start()
    {
        //playerState = PLAYER_STATE.E_NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        bool leftDown = Input.GetMouseButton(0);
        bool rightDown = Input.GetMouseButton(1);

        if (leftDown && rightDown )
        {
            ActivateBomb();
        }
        else if (leftDown)
        {
            ToggleShield(true);
            ToggleShadow(false);
        }
        else if (rightDown)
        {
            ToggleShadow(true);
            ToggleShield(false);
            //lamp.SetPower(-Time.deltaTime);
        }
        else
        {
            ToggleShield(false);
            ToggleShadow(false);
        }
        //else
        //{
        //    lamp.SetPower(Time.deltaTime);
        //}
    }

    private void ToggleShadow(bool active)
    {
        if (shadowActive != active)
        {
            shadowActive = active;
            if (shadowActive)
            {
                lamp.ActivateShadow();
                bodyCollider.enabled = false;
                shieldCollider.enabled = false;
            }
            else
            {
                lamp.DeactivateShadow();
                bodyCollider.enabled = true;
            }
        }
    }

    private void ToggleShield(bool active)
    {
        if ( shieldActive != active)
        {
            shieldActive = active;
            if (shieldActive)
            {
                lamp.ActivateShield();
                shieldCollider.enabled = true;
            }
            else
            {
                lamp.DeactivateShield();
                shieldCollider.enabled = false;
            }
        }        
    }

    private void ActivateBomb()
    {
        Debug.Log("Bomb activated");
    }

    void SetShadowMode(bool bShadow)
    {
    }
}
