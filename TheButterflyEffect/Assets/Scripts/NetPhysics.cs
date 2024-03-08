using UnityEngine;

public class NetPhysics : MonoBehaviour
{
    private Vector3 initialPos;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private float smooth = 10f;

    private void Start()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        if(Vector3.Distance(initialPos, transform.localPosition) > maxDistance)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, smooth * Time.deltaTime);
        }
    }
}
