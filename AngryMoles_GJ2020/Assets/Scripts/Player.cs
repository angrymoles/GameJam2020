using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character
{
    public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    public GameObject deathFXPrefab;
    public Sprite deathSprite;
    public int HP = 3;
    public int timeScoreMultiplier = 1;
    public int killScoreMultiplier = 5;
    public int shieldReflectScoreMultiplier = 2;
    public TMPro.TextMeshProUGUI scoreText;
    private int timeScore;
    private int killScore;
    private int shieldScore;
    private float levelStartTime;

    void Start()
    {
        playerMovement.moveSpeed = movementSpeed;
        currentHealth = MaxHealth;
        timeScore = 0;
        killScore = 0;
        shieldScore = 0;

        levelStartTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        var currTime = Time.timeSinceLevelLoad;
        timeScore = (int)(currTime - levelStartTime);

        if ( scoreText != null )
        {
            scoreText.SetText(GetTotalScore().ToString());
        }
    }

    public void ScoreKill()
    {
        ++killScore;
    }

    public void ScoreShield()
    {
        ++shieldScore;
    }

    public int GetTotalScore()
    {
        return ( timeScore * timeScoreMultiplier + killScore * killScoreMultiplier + shieldScore * shieldReflectScoreMultiplier );
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
