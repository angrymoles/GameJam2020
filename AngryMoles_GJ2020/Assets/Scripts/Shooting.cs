using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
 

    public float fireRate;

    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }

    IEnumerator ShootRoutine()
    {
       
            Shoot();
            yield return new WaitForSeconds(fireRate);
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position,firePoint.rotation);
               
    }

}
