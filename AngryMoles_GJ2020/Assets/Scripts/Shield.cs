using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Lamp lamp;
    void OnCollisionEnter2D(Collision2D collision)
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            var player = playerObj.GetComponent<Player>();
            player.ScoreShield();
        }
        lamp.PlayFX(collision.gameObject.transform.position, LampFXType.LampFX_ShieldHit);
    }
}
