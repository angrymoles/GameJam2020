using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float destoryHitEffectTime = 1f;
    public float lifeTime = 10f;
    public int damage = 1;

    private float timeSpan = 0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetContact(0).collider.transform.gameObject.name == "Barrier")
            {
                return;
            }

            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            player.Hit(damage);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemey = collision.gameObject.GetComponent<Enemy>();
            if (enemey == null)
            {
                return;
            }

            enemey.Hit(damage);
        }

        if (hitEffect == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, destoryHitEffectTime);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timeSpan = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        LifeTimeProcess();
    }

    void LifeTimeProcess()
    {
        timeSpan += Time.deltaTime;

        if (timeSpan > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
