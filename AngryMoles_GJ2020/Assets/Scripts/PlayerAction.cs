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
    public Sprite[] playerSprites;
    public Lamp lamp;
    public PolygonCollider2D bodyCollider;
    //private PLAYER_STATE playerState;
    private bool shadowActive = false;
    private bool shieldActive = false;    
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
        else
        {
            ToggleShield(false);
            ToggleShadow(false);
        }
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
                GetComponent<PlayerMovement>().moveSpeed = GetComponent<PlayerMovement>().darkMovespeed;
            }
            else
            {
                lamp.DeactivateShadow();
                bodyCollider.enabled = true;
                GetComponent<PlayerMovement>().moveSpeed = GetComponent<PlayerMovement>().lightMovespeed; 
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
            }
            else
            {
                lamp.DeactivateShield();
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
