using UnityEngine;

[CreateAssetMenu(fileName = "New Butterfly", menuName = "Butterfly")]
public class ButterflyData : ScriptableObject
{
    public string butterflyName;
    public string description;
    public GameObject modelPrefab; // Reference to the 3D model prefab

    public float size;

    public PairWith[] pairs; // Array to store the selection of races

    public ButterflyType type;

    // Add more variables as needed
}

[System.Serializable]
public class PairWith
{
    public ButterflyType type;
}

public enum ButterflyType
{
    Monarch,
    Swallowtail,
    BlueMorpho
}