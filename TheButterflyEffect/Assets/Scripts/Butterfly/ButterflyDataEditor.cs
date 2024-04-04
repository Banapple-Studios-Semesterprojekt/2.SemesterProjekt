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
    }
}