using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Patroler : MonoBehaviour
{
    private float currentSpeed; // �������� ����� ������
    public float chillSpeed; // �������� ��� ��������������
    public float angrySpeed; // �������� ����� ��� ��������

    public int positionOfPatrol;
    public Transform point;
    bool movingRight; // ��� �������� ���������� �����/������

    Transform player; // ��� ���������� ������� ������
    public float stoppingDistance; // ���������� �� ��������� �� �����

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


    // ��� ������ �� �������
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

    
    // ��� ����������, ���� ������ �������
    void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
    }


    // ����� �����, ���� ������ ������ �� �����
    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, currentSpeed * Time.deltaTime);
    }
}
