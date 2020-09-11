using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public EnemyPoints[] enemyPoints;
    public ArrayList DoorPoints;
    public GameObject EnemyPrefab;
    public float spawnTime = 3.0f;

    void Start()
    {
        DoorPoints = new ArrayList(3);
        DoorPoints.Add(transform.GetChild(0)); //Spawn
        DoorPoints.Add(transform.GetChild(1)); //Door
        DoorPoints.Add(transform.GetChild(2)); //Front
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
        Transform spawnPoint = (Transform)DoorPoints[0];
        GameObject enemy = Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.door = gameObject.GetComponent<Door>();
        int index = Random.Range(0, enemyPoints.Length -1);
        enemyMovement.enemyPoints = enemyPoints[index];
    }
}
