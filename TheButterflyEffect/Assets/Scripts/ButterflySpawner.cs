
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNumber
{
    public int min;
    public int max;

    public RangeNumber(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}

public class ButterflySpawner : MonoBehaviour
{
    public float circleRadius = 5f;
    public float maxButterfliesSpawn;
    public float spawnTime;
    int whatever = 0;

    Vector3 center;
    public ButterflyData[] butterflyPrefab;
    public List<GameObject> currentButterflies;
    public RangeNumber[] rangeNumber;

    private void Start()
    {
        StartCoroutine(SpawnButteflies());
        center = transform.position;

        rangeNumber= new RangeNumber[butterflyPrefab.Length];
        for (int i = 0; i < butterflyPrefab.Length; i++)
        {
            rangeNumber[i] = new RangeNumber(whatever, butterflyPrefab[i].spawnProbability+whatever);
            whatever += butterflyPrefab[i].spawnProbability;
        }

    }

    public IEnumerator SpawnButteflies()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);

            for (int i = 0; i < currentButterflies.Count; i++)
            {
                if(currentButterflies[i] == null)
                {
                    currentButterflies.Remove(currentButterflies[i]);
                }
            }

            if(currentButterflies.Count < maxButterfliesSpawn)
            {
                Vector3 spawnPosition = center + new Vector3(Random.Range(-circleRadius, circleRadius), Random.Range(3, 5), Random.Range(-circleRadius, circleRadius));
                // GameObject currentButterfly = Instantiate(butterflyPrefab[Random.Range(0, butterflyPrefab.Length)].modelPrefab, spawnPosition, Quaternion.identity);
                //currentButterflies.Add(currentButterfly);

                
                int r = Random.Range(0, whatever);
               
                for (int i = 0; i < butterflyPrefab.Length; i++)
                {
                    Debug.Log("rangeNumber[i].max"+ rangeNumber[i].max);
                    if (rangeNumber[i].max > r)
                    {
                        GameObject currentButterfly = Instantiate(butterflyPrefab[i].modelPrefab, spawnPosition, Quaternion.identity);
                       
                        currentButterflies.Add(currentButterfly);
                        break;
                    }
                } 
            }
            
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        
    }
}
