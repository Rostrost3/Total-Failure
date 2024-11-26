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

    [Header("Player Animation Settings")]
    public Animator animator;

    private float timeBtwAttack = 0f;
    public float startTimeBtwAttack; //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½

    public Transform attackPos; //ï¿½ï¿½ï¿½ï¿½, ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    public float attackRange; //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½
    public LayerMask whatIsEnemies;

    public double health = 10;
    public double damage = 1;

    public Transform groundCheckPos; //×òîáû ñìîòðåòü ÷òî ïîä èãðîêîì
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f); //Ðàçìåð
    public LayerMask spikesLayer; //Ìàñêà øèïîâ
    public LayerMask groundLayer; //Ìàñêà çåìëè

    private Vector2 playerPos; //Äëÿ çàïîìèíàíèÿ ïîçèöèè èãðîêà

    // Start is called before the first frame update
    void Start()
    {
        playerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        animator.SetBool("Attack", Input.GetMouseButtonDown(0));
    }

    private void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies); //Èùåì â ïîëå çðåíèè àòàêè èãðîêà âðàãîâ
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

    private void TouchSpikes()
    {
        //Åñëè íà øèïàõ
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, spikesLayer))
        {
            TakeDamage(3);
            transform.position = playerPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
