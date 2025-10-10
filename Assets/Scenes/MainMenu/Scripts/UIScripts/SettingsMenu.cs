using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    void Start()
    {
        
    }

}
