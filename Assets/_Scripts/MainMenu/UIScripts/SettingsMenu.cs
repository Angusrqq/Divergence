using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _refreshRateDropdown;
    [SerializeField] private TMP_Dropdown _fullScreenDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private int[] _refreshRates;
    private int _currentRefreshRateIndex = 0;
    private int _currentResolutionIndex = 0;
    private int _currentFullScreenIndex = 0;

    void Start()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        _refreshRates = new int[] { 30, 60, 75, 90, 120, 144, 165, 240, 300, 360, 500 };

        _resolutionDropdown.ClearOptions();
        _refreshRateDropdown.ClearOptions();
        _fullScreenDropdown.ClearOptions();

        float currentRefreshRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
        for (int i = 0; i < _resolutions.Length; i++)
        {
            int refresh = Mathf.RoundToInt((float)_resolutions[i].refreshRateRatio.value);
            if (refresh == currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> resolutionOptions = new();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + " x " + _filteredResolutions[i].height;
            resolutionOptions.Add(resolutionOption);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIndex = i;
            }
        }

        List<string> resfreshRateOptions = new();
        for (int i = 0; i < _refreshRates.Length; i++)
        {
            string refreshRateOption = _refreshRates[i].ToString() + " Hz";
            resfreshRateOptions.Add(refreshRateOption);
            if (_refreshRates[i] == currentRefreshRate)
            {
                _currentRefreshRateIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _fullScreenDropdown.AddOptions(new List<string> { "FullScreen", "Borderless", "Windowed" });

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                _currentFullScreenIndex = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                _currentFullScreenIndex = 1;
                break;
            case FullScreenMode.Windowed:
                _currentFullScreenIndex = 2;
                break;
        }
        
        _fullScreenDropdown.value = _currentFullScreenIndex;
        _fullScreenDropdown.RefreshShownValue();

        _refreshRateDropdown.AddOptions(resfreshRateOptions);
        _refreshRateDropdown.value = _currentRefreshRateIndex;
        _refreshRateDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetRefreshRate(int refreshRateIndex)
    {
        Application.targetFrameRate = _refreshRates[refreshRateIndex];
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
