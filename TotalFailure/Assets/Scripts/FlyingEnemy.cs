using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyClass, IDamageable //� ����� PlayerAttackAndHealth
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

    public double max_health = 10;
    public double current_health = 10;
    public double damage = 10;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //������� �� ����� ���������

    public Transform attackPos; //����, ��� ���� ������
    public float attackRange; //�������� �����

    [SerializeField] private EnemyHealthBar healthBar; // ����� ��������

    [Header("Player Animation Settings")]
    public Animator animator;


    // Start ���������� ����� ������ ������
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentSpeed = chillSpeed; // ��������� �������� - ��������������
        healthBar.SetHealthValue(current_health, max_health); // ��������� health bar
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

        if (current_health <= 0)
        {
            Destroy(gameObject);
        }

        IsMovingRight();

        Flip();

        Attack();

        animator.SetFloat("Speed", currentSpeed);
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
    public void TakeDamage(double damage)
    {
        current_health -= damage;
        healthBar.SetHealthValue(current_health, max_health);
    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //����� ���������
            Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, LayerMask.GetMask("Player"));
            if (playerToDamage != null)
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
