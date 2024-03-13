using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButterflyData))]
public class ButterflyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Cast the target to ButterflyData
        ButterflyData butterflyData = (ButterflyData)target;

        // Display default inspector GUI
        DrawDefaultInspector();

        // Add a field for the model prefab
        butterflyData.modelPrefab = (GameObject)EditorGUILayout.ObjectField("Model Prefab", butterflyData.modelPrefab, typeof(GameObject), false);

        EditorGUILayout.LabelField("Custom GUI Elements", EditorStyles.boldLabel);
        butterflyData.butterflyName = EditorGUILayout.TextField("Butterfly Name", butterflyData.butterflyName);
        butterflyData.description = EditorGUILayout.TextField("Description", butterflyData.description);

        butterflyData.size = EditorGUILayout.FloatField("Size", butterflyData.size);

        // You can add more custom GUI elements here if needed
    }
}