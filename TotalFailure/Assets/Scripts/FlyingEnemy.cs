using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float chillSpeed; // �������� ��������������
    public float angrySpeed; // �������� �������������
    public float stopDistance = 2.0f; // ���������� ��������� ��� �������������
    public float positionOfPatrol = 3.0f; // ��������� ��������������
    public Transform startingPoint; // ��������� ����� ��� ����������� � ��������������
    private GameObject player; // ������ �� ������

    private float currentSpeed; // ������� ��������
    private bool movingRight = true; // ����������� �������� ��� ��������������

    public bool chill = false; // ��������� ��������������
    public bool angry = false; // ��������� �������������
    public bool goBack = false; // ��������� ����������� � ��������� �����

    // Start ���������� ����� ������ ������
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentSpeed = chillSpeed; // ��������� �������� - ��������������
    }

    // Update ���������� ������ ����
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        // �������� ���������
        if (Vector2.Distance(transform.position, startingPoint.position) < positionOfPatrol && !angry)
        {
            chill = true;
            goBack = false;
        }

        IsMovingRight();
        Flip();
            
        // ���������� �������� � ����������� �� �������� ���������
        if (chill)
        {
            Chill();
            currentSpeed = chillSpeed;
        }
        else if (angry)
        {
            Chase();
            currentSpeed = angrySpeed;
        }
        if (goBack)
        {
            GoBack();
            currentSpeed = chillSpeed;
        }
    }

    private void IsMovingRight()
    {
        // �������������� �����-������
        if (transform.position.x > startingPoint.position.x + positionOfPatrol)
        {
            movingRight = false;
        }
        else if (transform.position.x < startingPoint.position.x - positionOfPatrol)
        {
            movingRight = true;
        }
    }

    private void Chase()
    {
        // ������������� ������
        if(Vector2.Distance(transform.position, player.transform.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
    }

    private void GoBack()
    {
        // ����������� � ��������� �����
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, currentSpeed * Time.deltaTime);
    }

    private void Chill()
    {
        // �������� � ����������� �� �����������
        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + currentSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - currentSpeed * Time.deltaTime, transform.position.y);
        }
    }

    private void Flip()
    {
        // ������� � ������� ������, ���� ���� ��������� � ��������� "angry"
        if (angry)
        {
            if ((transform.position.x < player.transform.position.x && transform.localScale.x < 0) ||
                (transform.position.x > player.transform.position.x && transform.localScale.x > 0))
            {
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        // ��������� ����� � ����������� �� ����������� ��������
        else if ((movingRight && transform.localScale.x < 0) || (!movingRight && transform.localScale.x > 0))
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }
}
