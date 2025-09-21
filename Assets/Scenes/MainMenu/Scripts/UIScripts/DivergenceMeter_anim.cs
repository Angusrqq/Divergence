using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class divergenceMeter_anim : MonoBehaviour
{
    private List<SpriteResolver> divergenceMeterResolvers;
    private List<DM_anim> divergenceMeterAnimScripts;
    private const string category = "DivergenceMeterSheet";
    public int seed;
    private float timer = 2;
    private float delay = 0;
    private int loops = 0;
    private float delayTime = 0;
    [SerializeField] private Material DM_material;
    private Color defaultMaterialColor;
    private float FadeTime = 1f;
    public bool isEnded = false;
    private bool EndAnimCalled = false;
    private SpriteResolver dotResolver;
    void Awake()
    {
        seed = PlayerPrefs.GetInt("Seed", Random.Range(0, 1999999));
        List<GameObject> divergenceMeterPositions = new List<GameObject>();
        divergenceMeterAnimScripts = new List<DM_anim>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("dot"))
            {
                SpriteResolver childResolver = child.GetComponent<SpriteResolver>();
                dotResolver = childResolver;
                //divergenceMeterAnimScripts.Add(child.GetComponent<DM_anim>());
                continue;
            }
            divergenceMeterPositions.Add(child.gameObject);
        }
        divergenceMeterResolvers = new List<SpriteResolver>();
        foreach (GameObject position in divergenceMeterPositions)
        {
            SpriteResolver resolver = position.GetComponent<SpriteResolver>();
            divergenceMeterResolvers.Add(resolver);
            DM_anim scriptRef = position.GetComponent<DM_anim>();
            divergenceMeterAnimScripts.Add(scriptRef);
        }
        defaultMaterialColor = DM_material.GetColor("_Color");

        Random.InitState(seed);
    }

    void FixedUpdate()
    {
        if (timer - Time.time < 0 && delayTime <= 0 && delay <= 0.04f)
        {
            delay += 0.00001f * loops;
            loops++;
            delayTime = delay;
        }
        delayTime -= Time.fixedDeltaTime;
        if (delayTime <= 0f && delay < 0.04f)
        {
            foreach (DM_anim script in divergenceMeterAnimScripts)
            {
                script.CustomUpdate();
            }
        }
        if (delay > 0.04f)
        {
            if (EndAnimCalled)
            {
                GlowFade(DM_material, DM_material.GetColor("_Color"), defaultMaterialColor, Time.fixedDeltaTime, ref FadeTime, ref isEnded, 1f, 0.6f);
            }
            else
            {
                EndAnim();
                EndAnimCalled = true;
            }
            if (isEnded)
            {
                this.enabled = false;
            }
        }
    }

    void EndAnim()
    {
        int offset = divergenceMeterResolvers.Count - seed.ToString().Length;
        for (int index = 0; index < offset; index++)
        {
            divergenceMeterResolvers[index].SetCategoryAndLabel(category, "0");
        }
        for (int index = offset; index < divergenceMeterResolvers.Count; index++)
        {
            divergenceMeterResolvers[index].SetCategoryAndLabel(category, seed.ToString().ToArray<char>()[index - offset].ToString());
        }
        dotResolver.SetCategoryAndLabel(category, "dot");
        DM_material.SetColor("_Color", defaultMaterialColor * 10);
        Debug.Log("Seed: " + seed);
        GameData.SetSeed(seed);
    }

    public static void GlowFade(Material material, Color StartMatColor, Color TargetMatColor, float deltatime,
                                ref float time, ref bool IsEnded, float lerp = 1f, float cutoff = 0f, bool isCyclic = false)
    {
        if (!isCyclic)
        {
            if (time <= cutoff)
            {
                IsEnded = true;
                material.SetColor("_Color", TargetMatColor);
            }
            else
            {
                time -= deltatime;
                material.SetColor("_Color", Color.Lerp(StartMatColor, TargetMatColor, 1 - (time / lerp)));
            }
        }
        if (time <= cutoff && isCyclic)
        {
            IsEnded = !IsEnded;
            time = lerp;
        }
        if (!IsEnded && isCyclic)
        {
            time -= deltatime;
            material.SetColor("_Color", Color.Lerp(StartMatColor, TargetMatColor, 1 - (time / lerp)));
        }
        if (IsEnded && isCyclic)
        {
            time -= deltatime;
            material.SetColor("_Color", Color.Lerp(TargetMatColor, StartMatColor, 1 - (time / lerp)));
        }
    }
}
