using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeartActivator : MonoBehaviour
{
    public List<Patroler> patrolers; // Список врагов
    private List<Vector2> positionPatrolers = new List<Vector2>();
    public GameObject heartObject; // Объект щита (к которому прикреплён скрипт PatrolerShield)
    private float chance;

    void Start()
    {
        heartObject.SetActive(false); // Начально скрываем щит
        chance = Random.Range(0f, 1f);
        foreach(var p in patrolers)
        {
            if(Random.value < chance)
            {
                p.isDropHeart = true;
            }
            positionPatrolers.Add(p.transform.position);
        }
    }

    void Update()
    {
        for(int i = 0; i < patrolers.Count(); i++)
        {
            if (patrolers[i] == null && patrolers[i].isDropHeart)
            {
                patrolers[i].isDropHeart = false;
                heartObject.SetActive(true);
                heartObject.transform.position = positionPatrolers[i];
            }
            else if (patrolers[i] != null)
            {
                positionPatrolers[i] = patrolers[i].transform.position;
            }
        }
    }
}
