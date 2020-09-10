using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float fireRateTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRateTime);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 dirction = PlayerTransform.position - firePoint.position;
        dirction.Normalize();

        rb.AddForce(dirction * bulletForce, ForceMode2D.Impulse);
    }
}
