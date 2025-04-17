using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static void SaveGame(PlayerAttackAndHealth player)
    {
        Vector2 pos = player.transform.position;

        PlayerPrefs.SetFloat("PlayerX", pos.x);
        PlayerPrefs.SetFloat("PlayerY", pos.y);
        PlayerPrefs.SetFloat("PlayerHealth", (float)player.current_health);
        PlayerPrefs.SetFloat("PlayerCountKeys", player.countOfKeys);
        PlayerPrefs.SetFloat("PlayerCountShots", player.CountOfShots);
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();
    }

    public static Vector2 LoadPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerX", 0f);
        float y = PlayerPrefs.GetFloat("PlayerY", 0f);
        return new Vector2(x, y);
    }

    public static string LoadSceneName()
    {
        return PlayerPrefs.GetString("SceneName", "LVL1.2"); // Уровень по умолчанию
    }

    public static List<float> LoadInfo()
    {
        return new List<float> { PlayerPrefs.GetFloat("PlayerHealth", 0f), PlayerPrefs.GetFloat("PlayerCountKeys", 0f), PlayerPrefs.GetFloat("PlayerCountShots", 0f) };
    }
}
