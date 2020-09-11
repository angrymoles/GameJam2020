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

    public float fireRate;

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
       
            Shoot();
            yield return new WaitForSeconds(fireRate);
        
    }


    private void FixedUpdate()
    {
        
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position,Quaternion.Euler(targetDirection));
               
    }

}
