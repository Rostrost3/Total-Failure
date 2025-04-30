using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class CharonCreateEnemies : MonoBehaviour
{
    public float delaySpawn = 1f;
    public int countOfSpawnEnemies = 3;
    public List<GameObject> spawnEnemies;
    public Transform spawnPoint;
    public CharonMovement charon;
    private List<EnemyClass> aliveEnemies = new List<EnemyClass>();
    private List<double> whenSpawn;

    // Start is called before the first frame update
    void Start()
    {
        whenSpawn = new List<double>();
        for(int i = 2; i <= 4; ++i)
        {
            whenSpawn.Add(charon.current_health / i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        aliveEnemies = aliveEnemies.Where(e => e != null).ToList();

        charon.IsFrozen = aliveEnemies.Count > 0;

        if (whenSpawn.Count > 0 && charon.current_health <= whenSpawn[0])
        {
            Spawn();
            whenSpawn.RemoveAt(0);
        }
    }

    void Spawn()
    {
        StartCoroutine(SpawnWithDelay());
    }

    IEnumerator SpawnWithDelay()
    {
        for (int j = 0; j < countOfSpawnEnemies; j++)
        {
            for (int i = 0; i < spawnEnemies.Count; ++i)
            {
                float randomOffsetX = Random.Range(-10f, 10f);
                float randomOffsetY = Random.Range(2f, 5f);

                GameObject minion = Instantiate(spawnEnemies[i], spawnPoint.transform.position, Quaternion.identity);
                if(minion.GetComponentInChildren<Patroler>() != null)
                {
                    Patroler patroler = minion.GetComponentInChildren<Patroler>();
                    aliveEnemies.Add(patroler);
                    patroler.point = spawnPoint.transform;
                    patroler.transform.position = spawnPoint.transform.position;
                }
                else
                {
                    FlyingEnemy flyingEnemy = minion.GetComponentInChildren<FlyingEnemy>();
                    ChaseControl chaseControl = minion.GetComponentInChildren<ChaseControl>();
                    aliveEnemies.Add(flyingEnemy);
                    Vector2 newPosition = new Vector2(spawnPoint.transform.position.x + randomOffsetX, spawnPoint.transform.position.y + randomOffsetY);
                    flyingEnemy.startingPoint.position = newPosition;
                    flyingEnemy.transform.position = newPosition;
                    chaseControl.transform.position = newPosition;
                }

                yield return new WaitForSeconds(delaySpawn);
            }
        }
    }
}
