using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class GlowOnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private TMP_Text tmp;
    private Button button;
    private Color defaultcolor;
    void Start()
    {
        button = GetComponent<Button>();
        tmp = GetComponentInChildren<TMP_Text>();
        defaultcolor = tmp.fontMaterial.GetColor("_GlowColor");
    }

    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tmp.fontMaterial.SetFloat("_GlowOffset", 1f);
        tmp.fontMaterial.SetFloat("_GlowPower", 1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tmp.fontMaterial.SetFloat("_GlowOffset", -0.39f);
        tmp.fontMaterial.SetFloat("_GlowPower", 0f);
    }

    public static float ElasticOut(float t, float p = 0.3f)
    {
        return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - p / 4f) * (2f * Mathf.PI) / p) + 1f;
    }
}
