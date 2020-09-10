using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 1;

    public void Hit(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
