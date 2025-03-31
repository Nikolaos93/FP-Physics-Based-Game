// This script is inspired by this youtube tutorial: https://www.youtube.com/watch?v=Cq_Nnw_LwnI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 2;
    [SerializeField] private Light sceneLight;
    [SerializeField] private Toggle fullScreenToggle;

    // Variables that store graphics settings data
    private int _qualityLevel;
    private bool _isFullScren;
    private float _brightnessLevel;


    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;


    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();


        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");

        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        sceneLight.intensity = _brightnessLevel;
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScren = isFullscreen;

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScren ? 1 : 0));
        Screen.fullScreen = _isFullScren;
        fullScreenToggle.isOn = _isFullScren;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;

        PlayerPrefs.SetInt("masterQuality   ", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

    }

    /*public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        // Change your brighntess with post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality   ", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScren ? 1 : 0));
        Screen.fullScreen = _isFullScren;

        //StartCoroutine (ConfirmationBox());
    }*/


    private void Awake()
    {
        /*_brightnessLevel = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
        brightnessSlider.value = _brightnessLevel;
        sceneLight.intensity = _brightnessLevel;

        _qualityLevel = PlayerPrefs.GetInt("masterQuality", 2);
        _isFullScren = PlayerPrefs.GetInt("masterFullScreen", 1) == 1;*/

        if (PlayerPrefs.HasKey("masterBrightness"))
        {
            _brightnessLevel = PlayerPrefs.GetFloat("masterBrightness");
            brightnessSlider.value = _brightnessLevel;
            sceneLight.intensity = _brightnessLevel;
        }
        else
        {
            _brightnessLevel = defaultBrightness;
            brightnessSlider.value = _brightnessLevel;
            sceneLight.intensity = _brightnessLevel;
        }

        if (PlayerPrefs.HasKey("masterQuality"))
        {
            _qualityLevel = PlayerPrefs.GetInt("masterQuality");
            QualitySettings.SetQualityLevel(_qualityLevel);
        }
        else
        {
            _qualityLevel = 2;
            QualitySettings.SetQualityLevel(_qualityLevel);
        }

        if (PlayerPrefs.HasKey("masterFullScreen"))
        {
            if (PlayerPrefs.GetInt("masterFullScreen") == 1)
            {
                _isFullScren = true;
                Screen.fullScreen = _isFullScren;
                fullScreenToggle.isOn = _isFullScren;
            }
            else
            {
                _isFullScren = false;
                Screen.fullScreen = _isFullScren;
                fullScreenToggle.isOn = _isFullScren;
            }
            
        }

    }
}
