using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private TimeController timeController;
    public GameObject enemyPrefab;

    private bool isNight;
    public float spawnHeight;
    public float spawnRadius;
    public int spawnLimit;
    public int count; //Monitors and increases no. of enemies until spawnLimit is reached.
    public float spawnDelay;
    private List<GameObject> enemyList; 

    private Coroutine SpawnEnemiesCoroutine;

    private void Start()
    {
        timeController = FindAnyObjectByType<TimeController>();

        timeController.onNight += onNight;

        enemyList = new List<GameObject>();
    }

    private void onNight(bool isNight)
    {
        this.isNight = isNight;
        
        if(SpawnEnemiesCoroutine == null && isNight)
        {
            print("heloo");
            SpawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }
        else if (!isNight)
        {
            foreach (GameObject enemy in enemyList)
            {
                Destroy(enemy);        
            }
            enemyList.Clear();
            count = 0; //Resets count to 0 for no of enemies to spawn.
        }

    }

    IEnumerator SpawnEnemies()
    {
        while(isNight && count < spawnLimit)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            pos.y = spawnHeight; //y-coordinate of Vector3 "pos" is set to "spawnHeight".
            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            count++;
            enemyList.Add(enemy);
            yield return new WaitForSeconds(Random.Range(spawnDelay / 2, spawnDelay));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}