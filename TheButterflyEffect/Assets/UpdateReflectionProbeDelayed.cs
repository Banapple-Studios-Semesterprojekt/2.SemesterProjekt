using UnityEngine;

public class UpdateReflectionProbeDelayed : MonoBehaviour
{
    [SerializeField] private float updateDelay = 5f;

    ReflectionProbe probe;
    private void Start()
    {
        probe = GetComponent<ReflectionProbe>();
        InvokeRepeating(nameof(UpdateProbe), updateDelay, updateDelay);
    }

    void UpdateProbe()
    {
        probe.RenderProbe();
    }
}
