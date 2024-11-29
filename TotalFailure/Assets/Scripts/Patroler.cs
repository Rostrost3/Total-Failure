using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Patroler : MonoBehaviour, IDamageable //В файле PlayerAttackAndHealth
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

    public double max_health = 10;
    public double current_health = 10;
    public double damage = 10;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //Сколько не может атаковать

    public Transform attackPos; //Круг, где ищем игрока
    public float attackRange; //Диапазон круга

    [SerializeField] private EnemyHealthBar healthBar; // Шкала здоровья


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar.SetHealthValue(current_health, max_health); // Активация health bar
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

        if(current_health <= 0)
        {
            Destroy(gameObject);
        }

        Attack();

        Flip();
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

    public void TakeDamage(double damage)
    {
        current_health -= damage;
        healthBar.SetHealthValue(current_health, max_health);

    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //Можно атаковать
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

    private void Flip()
    {
        // Поворот в сторону игрока, если враг находится в состоянии "angry"
        if (angry)
        {
            if ((transform.position.x < player.transform.position.x && transform.localScale.x > 0) ||
                (transform.position.x > player.transform.position.x && transform.localScale.x < 0))
            {
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        // Переворот врага в зависимости от направления движения
        else if ((movingRight && transform.localScale.x > 0) || (!movingRight && transform.localScale.x < 0))
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
