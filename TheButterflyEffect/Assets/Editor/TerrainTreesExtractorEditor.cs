using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainTreesExtractor))]
public class TerrainTreesExtractorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainTreesExtractor terrainTreesExtractor = (TerrainTreesExtractor)target;

        if(GUILayout.Button("Convert Terrain Trees"))
        {
            terrainTreesExtractor.Convert();
        }
    }
}
