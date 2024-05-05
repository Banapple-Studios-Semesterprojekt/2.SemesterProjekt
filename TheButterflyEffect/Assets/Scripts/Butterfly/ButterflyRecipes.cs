using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe")]
public class ButterflyRecipes : ScriptableObject
{
    public ButterflyData input1;
    public ButterflyData input2;
    public ButterflyData output;
    public float breedTime = 3f;
}
