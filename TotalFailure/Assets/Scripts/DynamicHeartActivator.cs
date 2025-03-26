using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicHeartActivator : MonoBehaviour
{
    public List<Patroler> patrolers; // Список врагов
    private List<Vector2> positionPatrolers = new List<Vector2>();
    public GameObject heartObject; // Объект щита (к которому прикреплён скрипт PatrolerShield)
    private float chance;

    void Start()
    {
        var allPatrolers = FindObjectsOfType<Patroler>();

        foreach (var patroler in allPatrolers)
        {
            patrolers.Add(patroler);
        }

        heartObject.SetActive(false); // Начально скрываем щит
        chance = Random.Range(0f, 0.5f);

        foreach (var p in patrolers)
        {
            if (Random.value < chance)
            {
                p.isDropHeart = true;
            }
            positionPatrolers.Add(p.transform.position);
        }
    }

    void Update()
    {
        var allPatrolers = FindObjectsOfType<Patroler>();
        foreach (var patroler in allPatrolers)
        {
            if (!patrolers.Contains(patroler))
            {
                patrolers.Add(patroler);
                if (Random.value < chance)
                {
                    patroler.isDropHeart = true;
                }
                positionPatrolers.Add(patroler.transform.position);
            }
        }

        for (int i = 0; i < patrolers.Count(); i++)
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
