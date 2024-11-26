using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Patroler : MonoBehaviour, IDamageable //� ����� PlayerAttackAndHealth
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

    public double health = 10;
    public double damage = 10;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //������� �� ����� ���������

    public Transform attackPos; //����, ��� ���� ������
    public float attackRange; //�������� �����


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

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        Attack();

        Flip();
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

    public void TakeDamage(double damage)
    {
        health -= damage;
    }

    private void Flip()
        {
            // ������� � ������� ������, ���� ���� ��������� � ��������� "angry"
            if ((transform.position.x < player.transform.position.x && transform.localScale.x < 0) ||
                (transform.position.x > player.transform.position.x && transform.localScale.x > 0))
            {
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //����� ���������
            Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, LayerMask.GetMask("Player"));
            if(playerToDamage != null)
            {
                playerToDamage.GetComponent<IDamageable>().TakeDamage(damage);
                Debug.Log("Enemy Attack!");
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
