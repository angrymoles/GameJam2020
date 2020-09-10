using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float destoryHitEffectTime = 1f;
    public float lifeTime = 10f;

    private float timeSpan = 0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitEffect == null)
        {
            return;
        }

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, destoryHitEffectTime);
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
