using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    void TakeDamage(double damage);
}

public interface IHealth
{
    void TakeHealth(double health);
}

public interface ITakeKeys
{
    void TakeKey();
}

public class PlayerAttackAndHealth : MonoBehaviour, IDamageable, IHealth, ITakeKeys
{
    public static bool GodMode = false;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //Сколько не может атаковать

    public Image bar; // Картинка с баром хп
    public float fill; // Хп в баре в процентах


    public Transform attackPos; //Круг, где ищем врагов
    public float attackRange; //Диапазон круга
    public LayerMask whatIsEnemies;

    public double max_health = 10;
    public double current_health = 10;
    //public bool isAlive = true;
    public double damage = 1;

    public Transform groundCheckPos; //Чтобы смотреть что под игроком
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); //Размер
    public LayerMask spikesLayer; //Маска шипов
    public LayerMask groundLayer; //Маска земли
    public DeathMenu deathMenu;
    public double spikedDamage = 3;

    private Vector2 checkpointPos; //Для запоминания позиции игрока

    public int countOfKeys = 0;

    //Счётик выстрелов
    public float CountOfShots;
    [SerializeField] private TextMeshProUGUI bulletsInfo;

    // Start is called before the first frame update
    void Start()
    {
        checkpointPos = transform.position;
        fill = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        if (current_health <= 0)
        { 
            Destroy(gameObject);
            //isAlive = false;
            deathMenu.MenuSetActive();
        }

        TouchSpikes();

        bulletsInfo.text = CountOfShots.ToString();
    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //Можно атаковать
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies); //Ищем в поле зрении атаки игрока врагов
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    IDamageable damageable = enemiesToDamage[i].GetComponent<IDamageable>();
                    if(damageable != null)
                    {
                        damageable.TakeDamage(damage);
                        Debug.Log("Attack!");
                        CountOfShots = 0; //После ближней атаки сбрасываем счётчик
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void TakeDamage(double damage)
    {
        if(GodMode)
        {
            return;
        }

        current_health -= damage;
        fill = (float)(current_health / max_health);
        bar.fillAmount = fill;
    }

    public void TakeHealth(double health)
    {
        if ((current_health + health) >= max_health)
        {
            current_health = max_health;
        }
        else
        {
            current_health += health;
        }
        fill = (float)(current_health / max_health);
        bar.fillAmount = fill;
    }

    private void TouchSpikes()
    {
        //Если на шипах
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, spikesLayer))
        {
            TakeDamage(spikedDamage);
            transform.position = checkpointPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void TakeKey()
    {
        countOfKeys++;

    }
    public void SpendKey() { countOfKeys = 0; }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }
}
