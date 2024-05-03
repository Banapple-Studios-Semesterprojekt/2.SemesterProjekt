
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySpawner : MonoBehaviour
{
    public ButterflyData[] butterflyPrefab;
    public float circleRadius = 5f;
    [SerializeField] float spawnchekradius = 2;
    public float maxButterfliesSpawn;
    public float spawnTime;
    // bliver brugt til at opbevare summen af alle sommerfuglens indivduelle sandsynligheder for at spawne 
    private int probabilitySum = 0;
    // bliver brugt til at obevare somerfulgeprfabsnes chance for at spawne
    private int[] rangeNumber;

    private Vector3 center;
    private ButterflyData butterflyData;
    private List<GameObject> currentButterflies;
    private TimeController timeController;

    private void Start()
    {
        timeController = FindAnyObjectByType<TimeController>();
 
        StartCoroutine(SpawnButterflies());
        center = transform.position;
        rangeNumber = new int[butterflyPrefab.Length];
        // vi k�re et for loop der s�tter ragnumber og probabiletysum
        for (int i = 0; i < butterflyPrefab.Length; i++)
        {
            //rangenumber s�tter vi til at v�re lig med sommfulgleprfabens spawnProbability pluds summen af alle forige sommerfugles spawnProbability
            rangeNumber[i] = butterflyPrefab[i].spawnProbability + probabilitySum;
            // probabilitySum s�tter vi til at v�rer lig med sommerfuglens spawnProbability 
            probabilitySum += butterflyPrefab[i].spawnProbability;
        }
    }

    public IEnumerator SpawnButterflies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            for (int i = 0; i < currentButterflies.Count; i++)
            {
                if (currentButterflies[i] == null)
                {
                    currentButterflies.Remove(currentButterflies[i]);
                }
            }

            if (currentButterflies.Count < maxButterfliesSpawn)
            {
                GeneratePosition();
            }

        }

    }

    public void GeneratePosition()
    {
        Vector3 spawnPosition = center + new Vector3(Random.Range(-circleRadius, circleRadius), Random.Range(3, 5), Random.Range(-circleRadius, circleRadius));

        chekposition(spawnPosition);
    }
    public void SpawnButterfly(Vector3 spawnPosition)
    {
        // vi får et tilfældigt nummer mellem nul og probabilitySum
        int r = Random.Range(0, probabilitySum);

        // vi kører så et for loop der k�re igenem alle butterflyPrefabsne for at finde ud af hvilken sommerfugl vi skal spawne på bagrund
        // r vi har fået 
        for (int i = 0; i < butterflyPrefab.Length; i++)
        {
            // vi sp�re om rangnumber er styrer end r hvis det er spawner vi den representative sommerfugl og stopper loppet
            // hvis ikke køre vi vider til det n�ste prefab og tjekker igen
            if (rangeNumber[i] > r)
            {
                if (!butterflyData.nightButterfly)
                {
                    GameObject currentButterfly = Instantiate(butterflyPrefab[i].itemObject, spawnPosition, Quaternion.identity);
                    currentButterflies.Add(currentButterfly);
                    break;
                }
                else if (butterflyData.nightButterfly) //Only spawn specific butterflies during the night, depends on a bool in butterflyData
                {
                    while(timeController.hour < 20 && timeController.hour > 6) 
                    {
                        GameObject nightButterfly = Instantiate(butterflyPrefab[i].itemObject, spawnPosition, Quaternion.identity);
                        currentButterflies.Add(nightButterfly);
                        break;
                    }
                    
                }

                //Code before my changes
                /*if (rangeNumber[i] < r)
                {
                    GameObject nightButterfly = Instantiate(butterflyPrefab[i].itemObject, spawnPosition, Quaternion.identity);
                    currentButterflies.Add(nightButterfly);
                    break;
                }*/
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);

    }

    public void chekposition(Vector3 v3)
    {

        RaycastHit hit;
        if (Physics.Raycast(v3 - new Vector3(spawnchekradius / 2, 0, 0), transform.right, out hit, spawnchekradius)
         || Physics.Raycast(v3 - new Vector3(0, spawnchekradius / 2, 0), transform.up, out hit, spawnchekradius)
         || Physics.Raycast(v3 - new Vector3(0, 0, spawnchekradius / 2), transform.forward, out hit, spawnchekradius)
         || Physics.Raycast(v3 - new Vector3(spawnchekradius / 2, 0, spawnchekradius / 2), new Vector3(1, 0, 1), out hit, spawnchekradius)
         || Physics.Raycast(v3 - new Vector3(-spawnchekradius / 2, 0, spawnchekradius / 2), new Vector3(-1, 0, 1), out hit, spawnchekradius))
        {
            GeneratePosition();
        }
        else
        {
            SpawnButterfly(v3);
        }

    }
}
