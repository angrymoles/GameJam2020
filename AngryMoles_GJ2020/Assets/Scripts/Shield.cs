using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Lamp lamp;
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        lamp.PlayFX(collision.gameObject.transform.position, LampFXType.LampFX_ShieldHit);
    }
}
