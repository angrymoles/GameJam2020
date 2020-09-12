using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 1;
    public Animator anim;
    

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            anim.SetTrigger("Dead");
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if ( playerObj )
            {
                var player = playerObj.GetComponent<Player>();
                player.ScoreKill();
            }
            Destroy(gameObject,10);
        }
    }
}
