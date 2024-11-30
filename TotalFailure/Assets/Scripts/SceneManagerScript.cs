using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{

    public void GameStart()
    {
        SceneManager.LoadScene("LVL1.2");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
