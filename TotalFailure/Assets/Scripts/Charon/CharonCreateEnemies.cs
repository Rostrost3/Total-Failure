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
                GameObject newPoint = new GameObject("PatrolerPoint1");
                newPoint.transform.position = new Vector2(spawnPoint.transform.position.x + j, spawnPoint.transform.position.y);

                GameObject minion = Instantiate(spawnEnemies[i], newPoint.transform.position, Quaternion.identity);
                if(minion.GetComponentInChildren<Patroler>() != null)
                {
                    Patroler patroler = minion.GetComponentInChildren<Patroler>();
                    aliveEnemies.Add(patroler);
                    patroler.point = newPoint.transform;
                    patroler.transform.position = newPoint.transform.position;
                }
                else
                {
                    FlyingEnemy flyingEnemy = minion.GetComponentInChildren<FlyingEnemy>();
                    aliveEnemies.Add(flyingEnemy);
                    flyingEnemy.startingPoint = newPoint.transform;
                    flyingEnemy.transform.position = newPoint.transform.position;
                }

                yield return new WaitForSeconds(delaySpawn);
            }
        }
    }
}
