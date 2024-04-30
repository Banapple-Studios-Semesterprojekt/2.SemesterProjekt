using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainGreen, terrainBlue, terrainRed;

    [SerializeField] private int countForBlueTerrain = 5, countForRedTerrain = 10;

    private void Start()
    {
        terrain = FindAnyObjectByType<Terrain>(FindObjectsInactive.Exclude);
        terrainGreen = Resources.Load<TerrainData>("Forest-Terrain");
        terrainBlue = Resources.Load<TerrainData>("ForestBlue-Terrain");
        terrainRed = Resources.Load<TerrainData>("ForestRed-Terrain");

        FindAnyObjectByType<BreedingSystem>().onBreed += BreedingSystem_OnBreed;
    }

    private void BreedingSystem_OnBreed(int breedCount)
    {
        ForestState state = breedCount >= countForRedTerrain ? ForestState.RedBiome : breedCount >= countForBlueTerrain ? ForestState.BlueBiome : ForestState.GreenBiome;
        StartCoroutine(ChangeForestState(state));
    }

    IEnumerator ChangeForestState(ForestState state)
    {
        BlackScreen.Instance().SetBlackScreen(true);
        yield return new WaitForSeconds(2f);
        switch(state)
        {
            case ForestState.GreenBiome:
                terrain.terrainData = terrainGreen;
                break;
            case ForestState.BlueBiome:
                terrain.terrainData = terrainBlue;
                break;
            case ForestState.RedBiome:
                terrain.terrainData = terrainRed;
                break;
        }
        BlackScreen.Instance().SetBlackScreen(false);
    }
}

public enum ForestState
{
    GreenBiome, BlueBiome, RedBiome
}
