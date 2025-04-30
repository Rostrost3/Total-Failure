using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerAttackAndHealth playerAttackAndHealth;
    public List<EnemyClass> enemies;
    public List<Key> keys;

    private void Awake()
    {
        playerAttackAndHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackAndHealth>();
    }

    private void Start()
    {
        int continueGame = PlayerPrefs.GetInt("ContinueGame", 0);
        if (continueGame == 1)
        {
            SaveSystem.LoadEnemies(enemies);
            SaveSystem.LoadKeys(keys);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttackAndHealth.UpdateCheckpoint(transform.position);
            SaveSystem.SavePosition(playerAttackAndHealth);
            SaveSystem.SavePlayer(playerAttackAndHealth);
            if (enemies[0].CompareTag("Charon"))
            {
                SaveSystem.SaveCharon(enemies[0].GetComponent<CharonMovement>());
            }
            else
            {
                SaveSystem.SaveEnemies(enemies);
            }
            SaveSystem.SaveKeys(keys);
        }
    }
}
