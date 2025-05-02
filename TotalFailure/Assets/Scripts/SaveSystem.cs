using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(PlayerAttackAndHealth player)
    {
        PlayerPrefs.SetFloat("PlayerHealth", (float)player.current_health);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "PlayerCountKeys", player.countOfKeys);
        PlayerPrefs.SetInt("PlayerCountShots", player.CountOfShots);
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();
    }

    public static void SavePosition(PlayerAttackAndHealth player)
    {
        Vector2 pos = player.transform.position;

        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "PlayerX", pos.x);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "PlayerY", pos.y);

        PlayerPrefs.Save();
    }

    public static void SaveKeys(List<Key> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Key" + i, keys[i] == null ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public static void SaveCharon(CharonMovement charon)
    {
        PlayerPrefs.SetInt("Charon", charon.isDead ? 1 : 0);
        PlayerPrefs.SetInt("CharonKey", charon.isDropKey ? 1 : 0);
        PlayerPrefs.SetFloat("CharonHealth", (float)charon.current_health);
    }

    public static void SaveEnemies(List<EnemyClass> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] != null)
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "Enemy" + i, (float)enemies[i].current_health);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "EnemyKey" + i, enemies[i].isDropKey ? 1 : 0);
            }
            else
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "Enemy" + i, 0);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "EnemyKey" + i, 0);
            }
        }
        PlayerPrefs.Save();
    }

    public static Vector2 LoadPosition()
    {
        float x = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "PlayerX", 0f);
        float y = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "PlayerY", 0f);
        return new Vector2(x, y);
    }

    public static void LoadKeys(List<Key> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "Key" + i))
            {
                if(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Key" + i) == 1) {
                    Destroy(keys[i].gameObject);
                }
            }
        }
    }

    public static Tuple<bool, bool, float> LoadCharon()
    {
        return Tuple.Create(PlayerPrefs.GetInt("Charon") == 1, PlayerPrefs.GetInt("CharonKey") == 1, PlayerPrefs.GetFloat("CharonHealth", 0));
    }

    public static void LoadEnemies(List<EnemyClass> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "Enemy" + i))
            {
                double currHealth = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "Enemy" + i);
                bool isDropKey = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "EnemyKey" + i) == 1;
                enemies[i].current_health = currHealth;
                enemies[i].isDropKey = isDropKey;
            }
        }
    }

    public static string LoadSceneName()
    {
        return PlayerPrefs.GetString("SceneName", "LVL1.2"); // Уровень по умолчанию
    }

    public static Tuple<float, int, int> LoadInfo()
    {
        return Tuple.Create(PlayerPrefs.GetFloat("PlayerHealth", 0), PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "PlayerCountKeys", 0), PlayerPrefs.GetInt("PlayerCountShots", 0));
    }
}
