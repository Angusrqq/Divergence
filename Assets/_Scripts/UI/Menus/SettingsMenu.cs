using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _refreshRateDropdown;
    [SerializeField] private TMP_Dropdown _fullScreenDropdown;

    [Header("Audio")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

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

        // Add resolution options to the dropdown
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

        // Add refresh rate options to the dropdown
        List<string> refreshRateOptions = new();
        for (int i = 0; i < _refreshRates.Length; i++)
        {
            string refreshRateOption = _refreshRates[i].ToString() + " Hz";
            refreshRateOptions.Add(refreshRateOption);

            if (_refreshRates[i] == currentRefreshRate)
            {
                _currentRefreshRateIndex = i;
            }
        }

        // Add resolution options to the dropdown
        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = _currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

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

        _fullScreenDropdown.AddOptions(new List<string> { "FullScreen", "Borderless", "Windowed" });
        _fullScreenDropdown.value = _currentFullScreenIndex;
        _fullScreenDropdown.RefreshShownValue();

        _refreshRateDropdown.AddOptions(refreshRateOptions);
        _refreshRateDropdown.value = _currentRefreshRateIndex;
        _refreshRateDropdown.RefreshShownValue();

        DefaultValues();
    }

    private void DefaultValues()
    {
        // Inverse of Mathf.Log10(volume) * 20f
        _masterVolumeSlider.value = Mathf.Pow(10f, AudioManager.instance.GetMasterVolume() / 20f);
        _musicVolumeSlider.value = Mathf.Pow(10f, AudioManager.instance.GetMusicVolume() / 20f);
        _sfxVolumeSlider.value = Mathf.Pow(10f, AudioManager.instance.GetSFXVolume() / 20f);
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

    public void SetMasterVolume()
    {
        AudioManager.instance.SetMasterVolume(Mathf.Log10(_masterVolumeSlider.value) * 20f);
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(Mathf.Log10(_musicVolumeSlider.value) * 20f);
    }

    public void SetSFXVolume()
    {
        AudioManager.instance.SetSFXVolume(Mathf.Log10(_sfxVolumeSlider.value) * 20f);
    }

    //TODO: tie methods above for sliders and this one to the sliders and a button respectfully and make it save the settings
    public void SaveSettings()
    {
        AudioManager.instance.Mixer.GetFloat("masterVolume", out float masterVolume);
        AudioManager.instance.Mixer.GetFloat("musicVolume", out float musicVolume);
        AudioManager.instance.Mixer.GetFloat("sfxVolume", out float sfxVolume);

        int ScreenWidth = Screen.width;
        int ScreenHeight = Screen.height;
        int RefreshRate = Application.targetFrameRate;
        string FullScreen = Screen.fullScreenMode.ToString();

        SettingsData data = new(
            masterVolume,
            musicVolume,
            sfxVolume,
            ScreenWidth,
            ScreenHeight,
            RefreshRate,
            FullScreen
        );

        GameData.UpdateSettings(data, true, true);
    }
    
    public void ResetSettings()
    {
        GameData.UpdateSettings(GameData.CurrentSettings, refreshSettings: true);
    }
}
