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
        E_DEAD,
    }

    public Lamp lamp;
    private PLAYER_STATE playerState;
    // Start is called before the first frame update
    void Start()
    {
        playerState = PLAYER_STATE.E_NORMAL;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            lamp.SetPower(-Time.deltaTime);
        }
        else
        {
            lamp.SetPower(Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lamp.ActiveBarrier(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            lamp.ActiveBarrier(true);
        }
    }

    void SetShadowMode(bool bShadow)
    {
    }
}
