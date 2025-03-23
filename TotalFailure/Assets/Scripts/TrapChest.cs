using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapChest : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    private bool isActivated = false;
    private static bool isEnemyAlive = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActivated && !isEnemyAlive)
        {
            ActivateTrap();
        }
    }

    void ActivateTrap()
    {
        isActivated = true;
        isEnemyAlive = true;

        float offset = 4f;

        for (int i = 0; i < 2; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + new Vector3(i * offset, 0, 0), spawnPoint.rotation);

            GameObject newPoint = new GameObject("PatrolerPoint1");
            newPoint.transform.position = spawnPoint.position + new Vector3(i * offset, 0, 0); // Смещаем точку

            Patroler patroler = enemy.GetComponent<Patroler>();
            patroler.point = newPoint.transform;
        }
    }

    public static void EnemyKilled()
    {
        isEnemyAlive = false;
    }
}
