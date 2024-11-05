using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField]
    Toggle FSSwitch = null;

    [SerializeField]
    TMP_Dropdown resolutionDropdown = null;

    [SerializeField]
    Slider volumeSlider = null;

    [SerializeField]
    TextMeshProUGUI volumeTextUI = null;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener((volume) =>
        {
            volumeTextUI.text = volume.ToString("0");
        });

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionsIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) 
        { 
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].height == Screen.height && resolutions[i].width == Screen.width)
            {
                currentResolutionsIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionsIndex;

        resolutionDropdown.RefreshShownValue();
    }


    public void ResoluitonChanger(int Index)
    {
        Resolution resolution = resolutions[Index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //public void FullScreenSwitch()
    //{
    //    FSSwitch.OnDeselect
    //}
}
