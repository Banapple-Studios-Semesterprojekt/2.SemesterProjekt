using UnityEngine;

public class Glowstick : ItemMechanic
{
    //Bug fixes:
    //When right clicking and then turning on glowstick --> Turns on both lights.
    private Material glowstickMat;
    private Color litColor;

    [SerializeField] private Light pointlight;
    [SerializeField] private Light spotlight;
    [SerializeField] private bool startOn = false;

    private bool isPointing;

    public delegate void PointAction(bool isPointing);
    public event PointAction onPoint;
    public delegate void ClickAction();
    public event ClickAction onClick;

    protected override void Awake()
    {
        base.Awake(); 
        glowstickMat = GetComponent<Renderer>().material;
        litColor = glowstickMat.GetColor("_EmissionColor");
        pointlight.enabled = startOn;
        spotlight.enabled = startOn;


        glowstickMat.SetColor("_EmissionColor", startOn ? litColor : Color.black);
    }

    private void Start()
    {

    }

    private void Spotlight()
    {
        //Turnary operator
        //Sets bool of isPointing to the opposite of what it currently is.
        isPointing = !isPointing;
        if (pointlight.enabled || spotlight.enabled)
        {   //Only enables light if turned on. 
            pointlight.enabled = !isPointing;
            spotlight.enabled = isPointing;
            glowstickMat.SetColor("_EmissionColor", isPointing ? Color.black : litColor);
        }
        onPoint?.Invoke(isPointing);
    }

    private void TurnOn()
    {
        //Checks if glowstick is turned on, and turns it on/off accordingly.
        if(pointlight.enabled || spotlight.enabled)
        {
            pointlight.enabled = false;
            spotlight.enabled = false;
            glowstickMat.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            pointlight.enabled = !isPointing;
            spotlight.enabled = isPointing;
            glowstickMat.SetColor("_EmissionColor", isPointing ? Color.black : litColor);
        }
        onClick?.Invoke();
    }

    private void OnEnable()
    {
        heldItemScript.onPrimaryAction += TurnOn;
        heldItemScript.onSecondaryAction += Spotlight;

    }
    private void OnDisable()
    {
        heldItemScript.onPrimaryAction -= TurnOn;
        heldItemScript.onSecondaryAction -= Spotlight;
    }
    private void OnDestroy()
    {
        heldItemScript.onPrimaryAction -= TurnOn;
        heldItemScript.onSecondaryAction -= Spotlight;
    }
}
