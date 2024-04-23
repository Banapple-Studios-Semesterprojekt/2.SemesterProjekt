using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundEffect : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private AudioSource source;
    private AudioClip click;
    private AudioClip hover;

    private void Start()
    {
        source = GameObject.Find("Button SFX").GetComponent<AudioSource>();
        click = AudioManager.Instance().data.popClick;
        hover = AudioManager.Instance().data.glassKnock;
    }

    public void OnPointerDown(PointerEventData data)
    {
        source.PlayClipWithRandomPitch(click);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        source.PlayClipWithRandomPitch(hover);
    }
}
