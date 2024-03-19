using FIMSpace.FProceduralAnimation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDirectionSpeed : MonoBehaviour
{
    public Vector3 direction { get; private set; }
    [SerializeField] private float speedMultiplier = 1f;
    private LegsAnimator legsAnimator;
    Vector3 lastPos;
    private Transform targetTransform;

    private void Start()
    {
        legsAnimator = GetComponent<LegsAnimator>();
        targetTransform = transform.FindFirstParent();
        lastPos = targetTransform.position;
    }

    private void Update()
    {
        direction = ((targetTransform.position - lastPos) * speedMultiplier) / Time.deltaTime;
        lastPos = targetTransform.position;
        if(legsAnimator)
        {
            legsAnimator.User_SetDesiredMovementDirection(direction);
        }

        if(Keyboard.current.iKey.wasPressedThisFrame)
        {
            legsAnimator.enabled = !legsAnimator.enabled;
        }
    }
}
