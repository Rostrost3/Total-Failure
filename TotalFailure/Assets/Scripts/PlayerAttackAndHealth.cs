using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(double damage);
}

public class PlayerAttackAndHealth : MonoBehaviour, IDamageable
{
    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //Сколько не может атаковать

    public Transform attackPos; //Круг, где ищем врагов
    public float attackRange; //Диапазон круга
    public LayerMask whatIsEnemies;

    public double health = 10;
    public double damage = 1;

    public Transform groundCheckPos; //Чтобы смотреть что под игроком
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); //Размер
    public LayerMask spikesLayer; //Маска шипов
    public LayerMask groundLayer; //Маска земли

    private Vector2 playerPos; //Для запоминания позиции игрока

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //Если игрок не на шипах, не на врагах и на земле, то запоминаем позицию игрока
        if (CheckMaskUnderPlayer() && Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            playerPos = transform.position;
        }
        else
        {
            TouchSpikes();
        }
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
        health -= damage;
    }

    private bool CheckMaskUnderPlayer()
    {
        //Проверяет, что игрок стоит не на шипах и не на враге
        Collider2D collider = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0);
        if(collider != null)
        {
            int objectLayer = collider.gameObject.layer;
            //Проверка, что не шипы и не враги
            if ((spikesLayer & (1 << objectLayer)) == 0 &&
                (whatIsEnemies & (1 << objectLayer)) == 0)
            {
                return true;
            }
        }
        return false;
    }

    private void TouchSpikes()
    {
        //Если на шипах
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, spikesLayer))
        {
            TakeDamage(health * 0.3);
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            transform.position = new Vector2(transform.position.x + 1f * direction, playerPos.y + 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
