using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerAttackAndHealth playerAttackAndHealth;

    private void Awake()
    {
        playerAttackAndHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackAndHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttackAndHealth.UpdateCheckpoint(transform.position);
            SaveSystem.SaveGame(playerAttackAndHealth);
        }
    }
}
