using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    enum ShootState
    {
        E_NONE,
        E_CHARGING,
        E_SHOOT,
    };

    public GameObject weapon;
    public Transform firePoint;
    public Transform ChargePosition;
    public GameObject bulletPrefab;
    private Rigidbody2D rb;
    public float bulletSurviveDuration=5;

    public float bulletForce = 20f;
    public float fireRate = 2.0f;

    public Transform target;
    public Vector3 targetDirection;

    public EnemyMovement enemyMovement;
    public int stopMaxFireCount = 3;
    public GameObject[] chargeEffects;
    public float destroyChargeEffectTime = 2f;
    private float chargetimeSpan = 0;
    private ShootState eShootState = ShootState.E_NONE;

    private int fireCount;


    // Start is called before the first frame update
    void Start()
    {
        eShootState = ShootState.E_NONE;
        target = FindObjectOfType<Player>().GetComponent<Transform>();
        rb =weapon.GetComponent<Rigidbody2D>();
        fireCount = 0;
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg+90;
        rb.rotation = angle;
        targetDirection.Normalize();
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (enemyMovement.GetMoveState() == EnemyMovement.MOVE_STATE.E_STOP
                || enemyMovement.GetMoveState() == EnemyMovement.MOVE_STATE.E_END)
            {
                if (eShootState == ShootState.E_NONE || eShootState == ShootState.E_CHARGING)
                {
                    Charge();
                    yield return null;
                }
                else if (eShootState == ShootState.E_SHOOT)
                {
                    Shoot();
                    yield return new WaitForSeconds(fireRate);
                }
            }
            else
            {
                yield return new WaitForSeconds(fireRate);
            }
        }
    }


    private void FixedUpdate()
    {

    }

    void Charge()
    {
        if (eShootState == ShootState.E_NONE)
        {
            eShootState = ShootState.E_CHARGING;
            int index = Random.Range(0, chargeEffects.Length);
            GameObject effect = Instantiate(chargeEffects[index], ChargePosition.position, Quaternion.identity);
            Destroy(effect, destroyChargeEffectTime);
            chargetimeSpan = 0f;
            effect.transform.parent = ChargePosition;
            return;
        }

        if (chargetimeSpan >= destroyChargeEffectTime)
        {
            eShootState = ShootState.E_SHOOT;
        }
        else
        {
            chargetimeSpan += Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (target == null)
        {
            return;
        }

        eShootState = ShootState.E_NONE;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Destroy(bullet, bulletSurviveDuration);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 dirction = target.position - firePoint.position;
        dirction.Normalize();
        rb.AddForce(dirction * bulletForce, ForceMode2D.Impulse);
        fireCount++;

        if (fireCount > stopMaxFireCount && enemyMovement.GetMoveState() != EnemyMovement.MOVE_STATE.E_END)
        {
            fireCount = 0;

            if (enemyMovement.bFinishedMove)
            {
                enemyMovement.SetMoveState(EnemyMovement.MOVE_STATE.E_CHASE);
            }
            else
            {
                enemyMovement.SetMoveState(EnemyMovement.MOVE_STATE.E_MOVE);
            }
        }
    }

}
