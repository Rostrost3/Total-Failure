using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(PlayerAttackAndHealth player)
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "PlayerHealth", (float)player.current_health);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "PlayerCountKeys", player.countOfKeys);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "PlayerCountShots", player.CountOfShots);
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

    public static void SaveEnemies(List<EnemyClass> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            // Сохраняем состояние каждого врага (мёртв или жив)
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Enemy" + i, enemies[i].isDead ? 1 : 0);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "EnemyKey" + i, enemies[i].isDropKey ? 1 : 0);
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

    public static void LoadEnemies(List<EnemyClass> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "Enemy" + i))
            {
                bool isDead = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Enemy" + i) == 1;
                bool isDropKey = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "EnemyKey" + i) == 1;
                enemies[i].isDead = isDead;
                enemies[i].isDropKey = isDropKey;
                if (isDead)
                {
                    Destroy(enemies[i].gameObject);  // Деактивируем врага, если он мертв
                }
            }
        }
    }

    public static string LoadSceneName()
    {
        return PlayerPrefs.GetString("SceneName", "LVL1.2"); // Уровень по умолчанию
    }

    public static Tuple<float, int, int> LoadInfo()
    {
        return Tuple.Create(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "PlayerHealth", 0f), PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "PlayerCountKeys", 0), PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "PlayerCountShots", 0));
    }
}
