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

    public GameObject HeartPrefab;
    public LayoutGroup healthBar;

    private List<GameObject> hearts = new List<GameObject>();

    private int timeScore;
    private int killScore;
    private int shieldScore;
    private float levelStartTime;

    private Audiomanager audioM;

    void Start()
    {
        playerMovement.moveSpeed = movementSpeed;
        currentHealth = MaxHealth;
        timeScore = 0;
        killScore = 0;
        shieldScore = 0;
        audioM = FindObjectOfType<Audiomanager>();

        levelStartTime = Time.timeSinceLevelLoad;

        for( int i = 0; i < HP; ++i )
        {
            var heart = Instantiate(HeartPrefab);
            heart.transform.parent = healthBar.transform;
            hearts.Add(heart);
        }
    }

    void Update()
    {
        var currTime = Time.timeSinceLevelLoad;
        timeScore = (int)(currTime - levelStartTime);

        if ( scoreText != null )
        {
            scoreText.SetText("Score: " + GetTotalScore().ToString());
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
            audioM.PlayOneTimeSound("Player_Death");
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
        else
        {
            for(int j = hearts.Count - 1; j > HP - 1; --j)
            {
                hearts[j].SetActive(false);
            }            
        }
    }

}
