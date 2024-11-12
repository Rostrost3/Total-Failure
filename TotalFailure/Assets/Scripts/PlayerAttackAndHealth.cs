using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

public class PlayerAttackAndHealth : MonoBehaviour, IDamageable
{
    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //Сколько не может атаковать

    public Transform attackPos; //Круг, где ищем врагов
    public float attackRange; //Диапазон круга
    public LayerMask whatIsEnemies;

    public int health = 10;
    public int damage = 1;

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
    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //Можно атаковать
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
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

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
