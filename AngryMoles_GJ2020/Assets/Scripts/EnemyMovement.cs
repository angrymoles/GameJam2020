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
        E_END,
    };

    public float enemySpeed = 1.0f;
    public Door door;
    public EnemyPoints enemyPoints;
    public Rigidbody2D rigidBody;
    private int curMovePoint;
    private MOVE_STATE moveState = MOVE_STATE.E_SPAWN_MOVE;

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
        moveState = MOVE_STATE.E_SPAWN_MOVE;
        curMovePoint = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveState == MOVE_STATE.E_SPAWN_MOVE)
        {
            SpawnMove();
        }
        else if (moveState == MOVE_STATE.E_MOVE)
        {
            MoveEnemyPoints();
        }
        else if (moveState == MOVE_STATE.E_STOP)
        {
            Stop();
        }
    }

    void SpawnMove()
    {
        if (curMovePoint > 2)
        {
            return;
        }

        Vector3 destPoint = ((Transform)(door.DoorPoints[curMovePoint + 1])).position;
        //SpawnPoint
        if (MoveEnemey(destPoint) && curMovePoint >= 2)
        {
            SetMoveState(MOVE_STATE.E_STOP);
            curMovePoint = 0;
        }
    }

    void MoveEnemyPoints()
    {
        ArrayList points = enemyPoints.EnemyMovePoints;
        if (curMovePoint >= points.Count)
        {
            Stop();
            SetMoveState(MOVE_STATE.E_END);
            return;
        }

        Vector3 destPoint = ((Transform)(points[curMovePoint])).position;
        //SpawnPoint
        if (MoveEnemey(destPoint))
        {
            SetMoveState(MOVE_STATE.E_STOP);
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
