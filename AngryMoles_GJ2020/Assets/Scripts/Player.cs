using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    public GameObject deathFXPrefab;
    public Sprite deathSprite;
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
            GameObject effect = Instantiate(deathFXPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if ( spriteRenderer != null )
            {
                spriteRenderer.sprite = deathSprite;
            }
            //GameManagerScript.Get().FadeToGameOver();
            SceneManager.LoadScene("Game_Over_Screen", LoadSceneMode.Additive);
        }
    }

}
