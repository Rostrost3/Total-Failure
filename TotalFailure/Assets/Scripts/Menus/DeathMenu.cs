using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject onDeathMenu;

    public void MenuSetActive()
    {
        onDeathMenu.SetActive(true);
    }

    public void PlayAgain()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
