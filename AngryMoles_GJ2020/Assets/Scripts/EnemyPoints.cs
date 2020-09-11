using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoints : MonoBehaviour
{
    public ArrayList EnemyMovePoints;
    void Start()
    {
        EnemyMovePoints = new ArrayList();
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child == transform)
            {
                continue;
            }
            EnemyMovePoints.Add(child);
        }
    }

    public ArrayList GetEnemyMovePoints()
    {
        return EnemyMovePoints;
    }
}
