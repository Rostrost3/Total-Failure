using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public enum MovementType { Vertical, Horizontal }
    public MovementType movementType = MovementType.Vertical; // Тип движения

    public float moveDistance = 7f; // Дистанция движения
    public float speed = 3f; // Скорость движения

    private Vector2 startPos;
    private bool movingForward = true;
    private Rigidbody2D rb;
    private Transform player = null;
    private Vector3 playerOffset = Vector3.zero;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>(); // Добавляем Rigidbody2D, если его нет
        }

        rb.bodyType = RigidbodyType2D.Kinematic; // Делаем платформу кинематической
        rb.gravityScale = 0; // Отключаем гравитацию
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Улучшает плавность движения
    }

    void FixedUpdate()
    {
        float distanceMoved = Vector2.Distance(startPos, rb.position);

        if (distanceMoved >= moveDistance)
        {
            movingForward = !movingForward;
        }

        Vector2 direction = movementType == MovementType.Vertical ? Vector2.up : Vector2.right;
        Vector2 targetVelocity = (movingForward ? direction : -direction) * speed;

        rb.velocity = targetVelocity; // Двигаем платформу через velocity

        // Если персонаж на платформе, двигаем его вместе с платформой
        if (player != null)
        {
            // Обновляем физику персонажа для его движения вдоль платформы
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 platformVelocity = rb.velocity;

            // Даем персонажу двигаться относительно платформы
            if (movementType == MovementType.Horizontal)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                playerRb.velocity = new Vector2(horizontalInput * speed + platformVelocity.x, playerRb.velocity.y);
            }
            else
            {
                // Если вертикальное движение платформы
                playerRb.velocity = new Vector2(playerRb.velocity.x, platformVelocity.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // Запоминаем персонажа
            playerOffset = player.position - transform.position; // Сохраняем смещение относительно платформы
            player.SetParent(transform); // Прикрепляем персонажа к платформе
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetParent(null); // Открепляем персонажа
            player = null;
        }
    }
}
