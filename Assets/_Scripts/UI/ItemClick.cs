using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClick : MonoBehaviour, IPointerUpHandler
{
    //Egor: CHANGE THIS!!! It's supposed to be an AudioSource, but when I tried to use it, it raise an error
    public AudioClip _onClick;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_onClick != null)
        {
            AudioManager.instance.PlaySFX(_onClick);
        }
    }
}
