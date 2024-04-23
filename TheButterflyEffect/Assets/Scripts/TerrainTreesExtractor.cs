#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TerrainTreesExtractor : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject treePrefab;

    public void Convert()
    {
        TerrainData data = terrain.terrainData;
        float width = data.size.x;
        float height = data.size.z;
        float y = data.size.y;

        Transform treesParent = Instantiate(new GameObject()).transform;
        treesParent.name = "Terrain Trees";

        foreach (TreeInstance tree in data.treeInstances)
        {
            Vector3 position = new Vector3(tree.position.x * width, tree.position.y * y, tree.position.z * height);
            GameObject currentTree = PrefabUtility.InstantiatePrefab(treePrefab, treesParent) as GameObject;
            currentTree.transform.position = position;
            currentTree.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
        }
    }
}
#endif