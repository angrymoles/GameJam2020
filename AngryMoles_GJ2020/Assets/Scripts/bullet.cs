using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering.Universal;

public class bullet : MonoBehaviour
{
    public GameObject[] hitEffect;
    public float destroyHitEffectTime = 2f;
    public float lifeTime = 300f;
    public int damage = 1;
    public float bulletSpeed = 5f;
    private Rigidbody2D rb;
    public GameObject myLight;

    //public UnityEngine.Experimental.Rendering.Universal.Light2D attachedLight;

    private float timeSpan = 0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 9 is the Player Layer
        if (collision.gameObject.layer==9)
        {
            if (collision.GetContact(0).collider.transform.gameObject.name == "Lamp")
            {
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
            GameObject effect = Instantiate(hitEffect[1], transform.position, Quaternion.identity);
            Destroy(effect, destroyHitEffectTime);
            Destroy(gameObject);
        }

        //11 Enemy
        if (collision.gameObject.layer == 11)
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
        bulletSpeed = Random.Range(3, 12);
        myLight.GetComponent<Light2D>().color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        GetComponent<SpriteRenderer>().color=Random.ColorHSV();
        //Debug.Log(bulletSpeed);
        //GetComponent<Rigidbody2D>().AddForce(transform.forward * bulletSpeed);
        //rb = GetComponent<Rigidbody2D>();
        //rb.velocity = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        //LifeTimeProcess();
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
