using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe")]
public class ButterflyRecipes : ScriptableObject
{
    public ButterflyData input1;
    public ButterflyData input2;
    public ButterflyData output;
    public float breedTime = 3f;
    
    public bool ContainsButterfly(ButterflyData butterfly)
    {
        if(butterfly == input1 || butterfly == input2)
        {
            return true;
        }
        return false;
    }
}
