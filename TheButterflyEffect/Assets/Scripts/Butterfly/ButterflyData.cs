using UnityEngine;

[CreateAssetMenu(fileName = "New Butterfly", menuName = "Butterfly")]
public class ButterflyData : Item
{
    public string butterflyName;
    public string description;

    public ButterflyTypeEnglish typeEnglish; //Enum for the English butterfly type
    public ButterflyTypeDanish typeDanish; //Enum for the Danish butterfly type

    //public GameObject modelPrefab; // Reference to the 3D model prefab

    public float size;
    public int spawnProbability;

    public PairWithEnglish[] pairsEnglish; // Array to store the English names butterflies that can be paired with
    public PairWithDanish[] pairsDanish; // Array to store the danish names butterflies that can be paired with
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