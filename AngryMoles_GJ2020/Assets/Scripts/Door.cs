using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject Spawnpoint;
    public GameObject DoorPoint;
    public GameObject TargetPoint;

    public Transform[] point;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;

    public ArrayList enemyPoints;
    public ArrayList DoorPoints;
    public GameObject EnemyPrefab;
    public float spawnTime = 3.0f;

    void Start()
    {
        DoorPoints = new ArrayList(3);
        DoorPoints.Add(Spawnpoint); //Spawn
        DoorPoints.Add(DoorPoint); //Door
        DoorPoints.Add(TargetPoint); //Front
       // enemyPoints = new ArrayList(3);       
        StartCoroutine(SpawnEnemyRoutine());
    }

    public ArrayList GetDoorPoints()
    {
        return DoorPoints;
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemey();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnEnemey()
    {
        //Transform spawnPoint = (Transform)DoorPoints[0];
        GameObject enemy = Instantiate(EnemyPrefab, Spawnpoint.transform.position, Spawnpoint.transform.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.door = gameObject.GetComponent<Door>();
        int index = Random.Range(0, point.Length -1);
       // enemyMovement.enemyPoints = point[index];
    }
}
