using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    

    void Start()
    {
        playerMovement.moveSpeed = movementSpeed;        
        currentHealth = MaxHealth;
    }

    void Update()
    {
        
    }


}
