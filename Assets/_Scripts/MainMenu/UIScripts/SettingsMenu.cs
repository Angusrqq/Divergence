using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshRateDropdown;
    [SerializeField] private TMP_Dropdown fullScreenDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int[] refreshRates;
    private int currentRefreshRateIndex = 0;
    private int currentResolutionIndex = 0;
    private int currentFullScreenIndex = 0;

    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        refreshRates = new int[] { 30, 60, 75, 90, 120, 144, 165, 240, 300, 360, 500 };

        resolutionDropdown.ClearOptions();
        fullScreenDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();

        float currentRefreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        for (int i = 0; i < resolutions.Length; i++)
        {
            int refresh = Mathf.RoundToInt((float)resolutions[i].refreshRateRatio.value);
            if (refresh == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            resolutionOptions.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        List<string> resfreshRateOptions = new List<string>();
        for (int i = 0; i < refreshRates.Length; i++)
        {
            string refreshRateOption = refreshRates[i].ToString() + " Hz";
            resfreshRateOptions.Add(refreshRateOption);
            if (refreshRates[i] == currentRefreshRate)
            {
                currentRefreshRateIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreenDropdown.AddOptions(new List<string> { "FullScreen", "Borderless", "Windowed" });
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                currentFullScreenIndex = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                currentFullScreenIndex = 1;
                break;
            case FullScreenMode.Windowed:
                currentFullScreenIndex = 2;
                break;
        }
        fullScreenDropdown.value = currentFullScreenIndex;
        fullScreenDropdown.RefreshShownValue();

        refreshRateDropdown.AddOptions(resfreshRateOptions);
        refreshRateDropdown.value = currentRefreshRateIndex;
        refreshRateDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetRefreshRate(int refreshRateIndex)
    {
        Application.targetFrameRate = refreshRates[refreshRateIndex];
    }
    
    public void SetFullscreen(int fullScreenIndex)
    {
        switch (fullScreenIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
}