using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClick : MonoBehaviour, IPointerUpHandler
{
    public AudioClip _onClick;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_onClick != null)
        {
            AudioManager.instance.PlaySFX(_onClick);
        }
    }
}
