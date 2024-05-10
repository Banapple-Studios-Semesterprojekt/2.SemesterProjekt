using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainGreen, terrainBlue, terrainRed;

    [SerializeField] private int countForBlueTerrain = 5, countForRedTerrain = 10;

    [SerializeField] private GameObject blueMushroomParent;
    [SerializeField] private GameObject redMushroomParent;

    private void Start()
    {
        terrain = FindAnyObjectByType<Terrain>(FindObjectsInactive.Exclude);
        terrainGreen = Resources.Load<TerrainData>("Game-Terrain");
        terrainBlue = Resources.Load<TerrainData>("GameBlue-Terrain");
        terrainRed = Resources.Load<TerrainData>("GameRed-Terrain");

        FindAnyObjectByType<BreedingSystem>().onBreed += BreedingSystem_OnBreed;
    }

    private void BreedingSystem_OnBreed(int breedCount)
    {
        if(breedCount == countForBlueTerrain)
        {
            StartCoroutine(ChangeForestState(ForestState.BlueBiome));
        }
        else if(breedCount == countForRedTerrain)
        {
            StartCoroutine(ChangeForestState(ForestState.RedBiome));
        }

    }

    IEnumerator ChangeForestState(ForestState state)
    {
        BlackScreen.Instance().SetBlackScreen(true);
        yield return new WaitForSeconds(2f);
        switch(state)
        {
            case ForestState.GreenBiome:
                terrain.terrainData = terrainGreen;
                blueMushroomParent.SetActive(false);
                redMushroomParent.SetActive(false);
                break;
            case ForestState.BlueBiome:
                terrain.terrainData = terrainBlue;
                blueMushroomParent.SetActive(true);
                redMushroomParent.SetActive(false);
                break;
            case ForestState.RedBiome:
                terrain.terrainData = terrainRed;
                redMushroomParent.SetActive(true);
                blueMushroomParent.SetActive(true);
                break;
        }
        BlackScreen.Instance().SetBlackScreen(false);
    }
}

public enum ForestState
{
    GreenBiome, BlueBiome, RedBiome
}