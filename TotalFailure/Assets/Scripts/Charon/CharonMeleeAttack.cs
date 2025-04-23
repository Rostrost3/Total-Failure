using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonMeeleAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackDelay = 1f;
    public int damage = 8;
    public GameObject player;
    public Transform attackPos;
    //public Animator animator;

    private bool isAttacking = false;

    void Update()
    {
        if (!isAttacking && Vector2.Distance(attackPos.position, player.transform.position) < attackRange)
        {
            isAttacking = true;
            StartCoroutine(MeleeAttack());
        }
    }

    IEnumerator MeleeAttack()
    {
        //animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        if (Vector2.Distance(attackPos.position, player.transform.position) < attackRange)
        {
            Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, LayerMask.GetMask("Player"));
            if (playerToDamage != null)
            {
                player.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }

        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
