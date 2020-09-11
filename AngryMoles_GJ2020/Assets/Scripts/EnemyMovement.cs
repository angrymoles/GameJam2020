using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MOVE_STATE
    {
        E_SPAWN_MOVE,
        E_MOVE,
        E_STOP,
        E_CHASE,
        E_END,
    };

    public float enemySpeed = 1.0f;
    public Door door;
    public EnemyPoints enemyPoints;
    public Rigidbody2D rigidBody;
    private int curMovePoint;
    private MOVE_STATE moveState = MOVE_STATE.E_SPAWN_MOVE;
    private Transform target;
    private Renderer enemyRenderer;
    private float chaseTime = 2.0f;
    private float curChaseTime = 0.0f;
    private const float chaseDistance = 2.0f;
    public bool bFinishedMove = false;

    public void SetMoveState(MOVE_STATE value)
    {
        moveState = value;
    }

    public MOVE_STATE GetMoveState()
    {
        return moveState;
    }

    void Start()
    {
        target = FindObjectOfType<Player>().GetComponent<Transform>();
        moveState = MOVE_STATE.E_SPAWN_MOVE;
        curMovePoint = 0;
        bFinishedMove = true;
        enemyRenderer = gameObject.GetComponent<Renderer>();
        enemyRenderer.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyPoints == null || door == null)
        {
            return;
        }

        if (moveState == MOVE_STATE.E_SPAWN_MOVE)
        {
            SpawnMove();
        }
        else if (moveState == MOVE_STATE.E_MOVE)
        {
            MoveEnemyPoints();
        }
        else if (moveState == MOVE_STATE.E_CHASE)
        {
            ChasePlayer();
        }
        else if (moveState == MOVE_STATE.E_STOP)
        {
            Stop();
        }

        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 lookDir = targetPosition - rigidBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270.0f;
        rigidBody.rotation = angle;
    }

    void SpawnMove()
    {
        if (curMovePoint > 2)
        {
            return;
        }

        Vector3 destPoint = ((Transform)(door.DoorPoints[curMovePoint + 1])).position;
        //SpawnPoint
        if (MoveEnemey(destPoint) )
        {
            if (curMovePoint >= 2)
            {
                SetMoveState(MOVE_STATE.E_MOVE);
                curMovePoint = 0;
            }
            else if (curMovePoint == 1)
            {
                enemyRenderer.enabled = true;
            }
        }
    }

    void MoveEnemyPoints()
    {
        ArrayList points = enemyPoints.EnemyMovePoints;
        if (curMovePoint >= points.Count)
        {
            bFinishedMove = true;
            SetMoveState(MOVE_STATE.E_CHASE);
            return;
        }

        Vector3 destPoint = ((Transform)(points[curMovePoint])).position;
        //SpawnPoint
        if (MoveEnemey(destPoint))
        {
            SetMoveState(MOVE_STATE.E_STOP);
        }
    }

    void ChasePlayer()
    {
        if (curChaseTime > chaseTime)
        {
            SetMoveState(MOVE_STATE.E_STOP);
            curChaseTime = 0.0f;
        }
        else
        {
            curChaseTime += Time.deltaTime;

            Vector2 dest = new Vector2(target.position.x, target.position.y);
            Vector2 dir = dest - rigidBody.position;
            if (Vector3.Magnitude(dir) > chaseDistance)
            {
                dir.Normalize();
                rigidBody.MovePosition(rigidBody.position + dir * enemySpeed * Time.fixedDeltaTime);
            }
            else
            {
                SetMoveState(MOVE_STATE.E_STOP);
                curChaseTime = 0.0f;
            }
        }
    }

    void Stop()
    {
        rigidBody.isKinematic = true;
        rigidBody.velocity = new Vector2(0, 0);
    }

    bool MoveEnemey(Vector3 destPosition)
    {
        Vector2 dest = new Vector2(destPosition.x, destPosition.y);
        Vector2 dir = dest - rigidBody.position;
        if (Vector3.Magnitude(dir) > enemySpeed/4)
        {
            dir.Normalize();
            rigidBody.MovePosition(rigidBody.position + dir * enemySpeed * Time.fixedDeltaTime);
            return false;
        }
        else
        {
            curMovePoint++;
            return true;
        }
    }
}
