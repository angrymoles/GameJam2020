using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 3;

    public void Hit(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
