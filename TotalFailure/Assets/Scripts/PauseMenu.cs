using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseGameMenu;
    private bool PauseGame;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseGame = false;
        Time.timeScale = 1;
        pauseGameMenu.SetActive(false);
    }
    public void Pause()
    {
        PauseGame = true;
        Time.timeScale = 0;
        pauseGameMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}