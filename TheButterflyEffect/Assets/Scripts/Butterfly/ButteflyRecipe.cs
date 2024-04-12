using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe")]
public class ButteflyRecipe : ScriptableObject
{
    public ButterflyData input1;
    public ButterflyData input2;
    public ButterflyData output;
    public float BreedTime;
}
