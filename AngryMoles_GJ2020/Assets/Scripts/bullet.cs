using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering.Universal;

public class bullet : MonoBehaviour
{
    public GameObject[] hitEffect;
    public float destroyHitEffectTime = 2f;
    public int damage = 1;
    private Rigidbody2D rb;
    public GameObject myLight;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 9 is the Player Layer
        if (collision.gameObject.layer==9 && gameObject.layer == 13)
        {
            if (collision.GetContact(0).collider.transform.gameObject.name == "Shield")
            {//Barrier
                gameObject.layer = 15;
                myLight.GetComponent<Light2D>().color = Color.blue;
                GetComponent<SpriteRenderer>().color = Color.blue;
                return;
            }

            GameObject effect = Instantiate(hitEffect[0], transform.position, Quaternion.identity);
            Destroy(effect, destroyHitEffectTime);

            Player player = collision.gameObject.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        //10 is the wall layer
        if (collision.gameObject.layer==10)
        {
            return;
        }

        //11 Enemy
        if (collision.gameObject.layer == 11 && gameObject.layer == 15)
        {
            GameObject effect = Instantiate(hitEffect[2], transform.position, Quaternion.identity);
            Destroy(effect, destroyHitEffectTime);

            Enemy enemey = collision.gameObject.GetComponent<Enemy>();
            if (enemey == null)
            {
                return;
            }
            enemey.TakeDamage(damage);
            Destroy(gameObject);
        }

        //12 shield 13 bullet
        if (collision.gameObject.layer == 12)
        {
            GameObject effect = Instantiate(hitEffect[3], transform.position, Quaternion.identity);
            Destroy(effect, destroyHitEffectTime);

        }

        if (collision.gameObject.layer == 13)
        {
            return;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        myLight.GetComponent<Light2D>().color = Color.red;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
