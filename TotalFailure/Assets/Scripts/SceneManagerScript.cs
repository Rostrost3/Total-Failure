using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using System.Threading;

public class SceneManagerScript : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [SerializeField] Slider volumeSlider = null;
    [SerializeField] TextMeshProUGUI volumeTextUI = null;

    [SerializeField] private Toggle godModeToggle;

    [SerializeField] public GameObject imagePanel;
    [SerializeField] public Image imageFrame;
    [SerializeField] public Sprite firstImage;
    [SerializeField] public Sprite secondImage;
    public GameObject SettingsMenu;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionsIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionsIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionsIndex);

        volumeTextUI.text = volumeSlider.value.ToString();

        volumeSlider.onValueChanged.AddListener((volume) =>
        {
            volumeTextUI.text = volume.ToString("0");
        });

        godModeToggle.onValueChanged.AddListener(SetGodMode);
    }

    private IEnumerator PlayCutsceneAndStartGame()
    {
        imagePanel.SetActive(true);
        imageFrame.sprite = firstImage;

        yield return new WaitForSeconds(7f);

        imageFrame.sprite = secondImage;

        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene("LVL1.2");
    }

    public void GameStart()
    {
        if (!SettingsMenu.activeSelf)
        {
            SettingsMenu.SetActive(true);
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ContinueGame", 0);

        // Запускаем кат-сцену
        StartCoroutine(PlayCutsceneAndStartGame());
    }

    public void ToSettings()
    {
        SettingsMenu.SetActive(true);
    }

    public void ToMainMenu()
    {
        SettingsMenu.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    private void SetGodMode(bool isOn)
    {
        PlayerAttackAndHealth.GodMode = isOn;
        PlayerPrefs.SetInt("GodMode", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("fullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
        }

        if (PlayerPrefs.HasKey("fullScreenPreference"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("fullScreenPreference"));
        }
        else
        {
            Screen.fullScreen = true;
        }

        bool savedGodMode = PlayerPrefs.GetInt("GodMode", 0) == 1;
        godModeToggle.isOn = savedGodMode; // Устанавливаем Toggle
        PlayerAttackAndHealth.GodMode = savedGodMode;
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("ContinueGame", 1);
        string sceneToLoad = SaveSystem.LoadSceneName();
        SceneManager.LoadScene(sceneToLoad);
    }
}