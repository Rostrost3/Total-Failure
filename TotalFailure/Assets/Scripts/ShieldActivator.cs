using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldActivator : MonoBehaviour
{
    public List<Patroler> patrolers; // Список врагов
    private int index; // Индекс выбранного врага
    private Vector2 positionPatroler;
    public GameObject shieldObject; // Объект щита (к которому прикреплён скрипт PatrolerShield)
    private bool stop;

    void Start()
    {
        index = Random.Range(0, patrolers.Count); // Выбираем случайного врага
        shieldObject.SetActive(false); // Начально скрываем щит
        stop = false;
    }

    void Update()
    {
        if (!stop)
        {
            // Проверяем, умер ли враг
            if (patrolers[index] == null)
            {
                stop = true;
                shieldObject.SetActive(true); // Делаем щит видимым
                shieldObject.transform.position = positionPatroler; // Размещаем щит на месте врага
            }
            else
            {
                positionPatroler.x = patrolers[index].transform.position.x + 1;
                positionPatroler.y = patrolers[index].transform.position.y;
            }
        }
    }
}
