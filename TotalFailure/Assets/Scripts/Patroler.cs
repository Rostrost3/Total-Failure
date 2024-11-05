using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Patroler : MonoBehaviour
{
    private float currentSpeed; // Скорость врага сейчас
    public float chillSpeed; // Скорость при патрулировании
    public float angrySpeed; // Скорость врага при агрессии

    public int positionOfPatrol;
    public Transform point;
    bool movingRight; // Для поворота противника влево/вправо

    Transform player; // Для считывания позиции игрока
    public float stoppingDistance; // Расстояние от противнка до героя

    bool chill = false;
    bool angry = false;
    bool goBack = false;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
        {
            chill = true;
        }

        
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
        }


        if (chill == true)
        {
            Chill();
            currentSpeed = chillSpeed;
        }
        else if (angry == true)
        {
            Angry();
            currentSpeed = angrySpeed;
        }
        else if (goBack == true)
        {
            GoBack();
            currentSpeed = chillSpeed;
        }
    }


    // Что делает на дефолте
    void Chill()
    {
        if (transform.position.x > point.position.x + positionOfPatrol)
        {
            movingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + currentSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - currentSpeed * Time.deltaTime, transform.position.y);
        }
    }

    
    // Что происходит, если близко подойти
    void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
    }


    // Отход назад, если далеко отошел от врага
    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, currentSpeed * Time.deltaTime);
    }
}
