using System.Collections.Generic;
using UnityEngine;

public class DivergenceMeter_Idle : MonoBehaviour
{
    private List<DM_anim> divergenceMeterAnimScripts;
    [SerializeField] private Material DM_material;
    private Color defaultMaterialColor;
    private float FadeTime = 2f;
    private bool isEnded = false;
    private float delay = 1f;
    void Awake()
    {
        List<GameObject> divergenceMeterPositions = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("dot"))
            {
                continue;
            }
            divergenceMeterPositions.Add(child.gameObject);
        }
        divergenceMeterAnimScripts = new List<DM_anim>();
        foreach (GameObject position in divergenceMeterPositions)
        {
            DM_anim scriptRef = position.GetComponent<DM_anim>();
            divergenceMeterAnimScripts.Add(scriptRef);
        }
        defaultMaterialColor = DM_material.GetColor("_Color");
    }

    void FixedUpdate()
    {
        if (delay > 0)
        {
            delay -= Time.fixedDeltaTime;
        }
        if (delay <= 0)
        {
            foreach (DM_anim script in divergenceMeterAnimScripts)
            {
                script.CustomUpdate();
            }
            delay = 1f;
        }
        divergenceMeter_anim.GlowFade(DM_material, defaultMaterialColor, defaultMaterialColor * 5f, Time.fixedDeltaTime, ref FadeTime, ref isEnded, 2f, 0f, true);
    }

    public void OnDisable()
    {
        DM_material.SetColor("_Color", defaultMaterialColor);
    }
}
