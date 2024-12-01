using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public GameObject onWinMenu;
    public List<Patroler> patrolers;
    public List<FlyingEnemy> flyEnemies;

    // Update is called once per frame
    void Update()
    {
        patrolers = patrolers.Where(x => x != null).ToList();
        flyEnemies = flyEnemies.Where(x => x != null).ToList();

        if (patrolers.Count() + flyEnemies.Count() == 0)
        {
            onWinMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            onWinMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
