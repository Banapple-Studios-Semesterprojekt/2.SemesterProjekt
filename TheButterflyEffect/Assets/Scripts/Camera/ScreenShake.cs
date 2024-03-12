using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    //public bool start = false;
    public AnimationCurve curve;
    public float duration = 1.0f;
    
    /*private void Update()
    {
        if (start) //if player stops running then activate screenshake 
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }*/
   
    public IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {  
            Vector3 startPosition = transform.position;
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
    }
}
