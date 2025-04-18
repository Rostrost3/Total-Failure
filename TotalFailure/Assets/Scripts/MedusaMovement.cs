using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaMovement : EnemyClass, IDamageable
{
    private float currentSpeed;
    public float chillSpeed;

    public int positionOfPatrol;
    public Transform point;
    bool movingRight;

    Transform player;
    public float stoppingDistance;

    bool chill = true;
    bool goBack = false;

    public bool IsShooting = false;

    public double max_health = 10;
    public double current_health = 10;

    [SerializeField] private EnemyHealthBar healthBar;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [Header("Player Animation Settings")]
    public Animator animator;

    //public WinMenu menu;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar.SetHealthValue(current_health, max_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
        }
        else
        {
            goBack= false;
        }

        if (chill == true)
        {
            Chill();
            currentSpeed = chillSpeed;
        }
        else if (goBack == true)
        {
            GoBack();
            currentSpeed = chillSpeed;
        }

        if (current_health <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }

        Flip();

        animator.SetBool("isShooting", IsShooting);

        //animator.SetBool("Attack", isAttack);
    }


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

    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, currentSpeed * Time.deltaTime);
    }

    public void TakeDamage(double damage)
    {
        current_health -= damage;
        healthBar.SetHealthValue(current_health, max_health);

    }

    private void Flip()
    {
        if (IsShooting)
        {
            if ((player.position.x < transform.position.x && transform.localScale.x > 0) ||
                (player.position.x > transform.position.x && transform.localScale.x < 0))
            {
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
        else
        {
            if ((movingRight && transform.localScale.x < 0) || (!movingRight && transform.localScale.x > 0))
            {
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
