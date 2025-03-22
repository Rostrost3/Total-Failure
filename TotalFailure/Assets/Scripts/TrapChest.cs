using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapChest : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб противника
    public Transform spawnPoint; // Точка спавна противника
    private bool isActivated = false; // Флаг активации сундука
    private static bool isEnemyAlive = false; // Флаг наличия живого противника

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isActivated && !isEnemyAlive)
        {
            ActivateTrap();
        }
    }

    void ActivateTrap()
    {
        isActivated = true;
        isEnemyAlive = true;
        Instantiate(enemyPrefab, (Vector2)spawnPoint.position, spawnPoint.rotation);
    }

    public static void EnemyKilled()
    {
        isEnemyAlive = false;
    }
}
