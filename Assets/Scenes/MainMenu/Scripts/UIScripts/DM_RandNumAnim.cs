using UnityEngine;
using UnityEngine.U2D.Animation;

public class DM_anim : MonoBehaviour
{
    private SpriteResolver resolver;
    private const string category = "DivergenceMeterSheet";

    void Awake()
    {
        resolver = GetComponent<SpriteResolver>();
    }
    public void CustomUpdate()
    {
        int rand = Random.Range(0, 10);
        resolver.SetCategoryAndLabel(category, rand.ToString());
    }
}
