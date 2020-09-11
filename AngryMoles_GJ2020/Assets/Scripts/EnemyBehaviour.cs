using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject weapon;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody2D rb;

    public float bulletForce = 20f;
    public float fireRate = 2.0f;

    public Transform target;
    public Vector3 targetDirection;


    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().GetComponent<Transform>();
        rb =weapon.GetComponent<Rigidbody2D>();
        StartCoroutine(ShootRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg+90;
        rb.rotation = angle;
        targetDirection.Normalize();
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }


    private void FixedUpdate()
    {

    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 dirction = target.position - firePoint.position;
        dirction.Normalize();
        rb.AddForce(dirction * bulletForce, ForceMode2D.Impulse);
    }

}
