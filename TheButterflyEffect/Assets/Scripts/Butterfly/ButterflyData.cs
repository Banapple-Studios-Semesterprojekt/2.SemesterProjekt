using UnityEngine;

[CreateAssetMenu(fileName = "New Butterfly", menuName = "Butterfly")]
public class ButterflyData : Item
{
    public string butterflyName;
    [TextArea(5, 10)] public string description;

    public ButterflyTypeEnglish typeEnglish; //Enum for the English butterfly type
    public ButterflyTypeDanish typeDanish; //Enum for the Danish butterfly type

    //public GameObject modelPrefab; // Reference to the 3D model prefab

    public float size;
    public int spawnProbability;
}

[System.Serializable]
public class PairWithEnglish
{
    public ButterflyTypeEnglish typeEnglish;
}

[System.Serializable]
public class PairWithDanish
{
    public ButterflyTypeDanish typeDanish;
}

public enum ButterflyTypeEnglish
{
    Monarch,
    RedAdmiral,
    CommonBrimstone,
    HollyBlue,
    TriangleBirdwing
}

public enum ButterflyTypeDanish
{
    Monark,
    Admiral,
    Citronsommerfugl,
    Skovblåfugl,
    TrogonopteraTrojana //Trekantfuglevingen, der findes ikke en dansk oversættelse
}