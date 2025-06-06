using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonMovement : EnemyClass, IDamageable
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

    public bool IsFrozen = false;

    public double max_health;

    [SerializeField] private EnemyHealthBar healthBar;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);

    [Header("Player Animation Settings")]
    public Animator animator;

    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Tuple<bool, bool, float> info = SaveSystem.LoadCharon();
        isDead = PlayerPrefs.HasKey("Charon") ? info.Item1 : false;
        isDropKey = PlayerPrefs.HasKey("CharonKey") ? info.Item2 : false;
        current_health = PlayerPrefs.HasKey("CharonHealth") ? info.Item3 : max_health;
        healthBar.SetHealthValue(current_health, max_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFrozen) return;

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
            Die();
        }

        Flip();

        //animator.SetBool("isShooting", IsShooting);

        //animator.SetBool("Attack", isAttack);
    }

    public override void Die(bool fromLoad = false)
    {
        isDead = true;

        Destroy(gameObject);

        menu.SetActive(true);
        Time.timeScale = 0;
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
        if (IsFrozen) return;

        current_health -= damage;
        healthBar.SetHealthValue(current_health, max_health);

    }

    private void Flip()
    {
        if ((movingRight && transform.localScale.x < 0) || (!movingRight && transform.localScale.x > 0))
        {
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
