using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    public int HP = 3;

    void Start()
    {
        playerMovement.moveSpeed = movementSpeed;
        currentHealth = MaxHealth;
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
