using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapChest : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float activationDistance = 3f; // Максимальное расстояние для активации

    public bool isActivated = false;
    public static bool isEnemyAlive = false;
    private Transform playerTransform;

    public bool isDropKey = false;


    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // Получаем ссылку на игрока
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(playerTransform.position, transform.position) <= activationDistance)
        {
            UIMessageManager.Instance.ShowMessage("Press E to open the chest");
        }
        if (Vector3.Distance(playerTransform.position, transform.position) <= activationDistance &&
            Input.GetKeyDown(KeyCode.E) && !isActivated && !isEnemyAlive)
        {
            if(!isDropKey)
            {
                ActivateTrap();
            }
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
