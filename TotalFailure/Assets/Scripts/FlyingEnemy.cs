using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IDamageable //В файле PlayerAttackAndHealth
{
    public float chillSpeed; // Скорость патрулирования
    public float angrySpeed; // Скорость преследования
    public float stopDistance = 2.0f; // Расстояние остановки при преследовании
    public float positionOfPatrol = 3.0f; // Дистанция патрулирования
    public Transform startingPoint; // Начальная точка для возвращения и патрулирования
    private GameObject player; // Ссылка на игрока

    private float currentSpeed; // Текущая скорость
    private bool movingRight = true; // Направление движения при патрулировании

    public bool chill = false; // Состояние патрулирования
    public bool angry = false; // Состояние преследования
    public bool goBack = false; // Состояние возвращения к начальной точке

    public double health = 10;
    public double damage = 10;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //Сколько не может атаковать

    public Transform attackPos; //Круг, где ищем игрока
    public float attackRange; //Диапазон круга

    // Start вызывается перед первым кадром
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentSpeed = chillSpeed; // Начальная скорость - патрулирование
    }

    // Update вызывается каждый кадр
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        // Проверка состояний
        if (Vector2.Distance(transform.position, startingPoint.position) < positionOfPatrol && !angry)
        {
            chill = true;
            goBack = false;
        }

        IsMovingRight();
        Flip();
            
        // Выполнение действий в зависимости от текущего состояния
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

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Attack();
    }

    private void IsMovingRight()
    {
        // Патрулирование влево-вправо
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
        // Преследование игрока
        if(Vector2.Distance(transform.position, player.transform.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
    }

    private void GoBack()
    {
        // Возвращение к начальной точке
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, currentSpeed * Time.deltaTime);
    }

    private void Chill()
    {
        // Движение в зависимости от направления
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
        // Поворот в сторону игрока, если враг находится в состоянии "angry"
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
        // Переворот врага в зависимости от направления движения
        else if ((movingRight && transform.localScale.x < 0) || (!movingRight && transform.localScale.x > 0))
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }
    public void TakeDamage(double damage)
    {
        health -= damage;
    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //Можно атаковать
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
