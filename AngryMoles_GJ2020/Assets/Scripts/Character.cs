using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MaxHealth = 1f;
    public float movementSpeed = 5f;

    protected float currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        var collidedObject = collision.gameObject;
        var bullet = collidedObject.GetComponent<bullet>();
        if (bullet)
        {
            TakeDamage(bullet.damage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
