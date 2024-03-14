using UnityEngine;
using MilkShake;

public class ShakeTrigger : MonoBehaviour
{
    [SerializeField] private ShakePreset preset;

    private void OnTriggerEnter(Collider other)
    {
        Shaker.ShakeAll(preset);
    }
}
