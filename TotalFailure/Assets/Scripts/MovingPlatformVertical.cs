using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public enum MovementType { Vertical, Horizontal }
    public MovementType movementType = MovementType.Vertical;
    public float moveDistance = 7f;
    public float speed = 3f;

    private Vector2 startPos;
    private bool movingForward = true;
    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private GameObject playerOnPlatform; // Игрок на платформе

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        lastPosition = rb.position;
    }

    void FixedUpdate()
    {
        // Движение платформы
        float distanceMoved = Vector2.Distance(startPos, rb.position);
        if (distanceMoved >= moveDistance)
            movingForward = !movingForward;

        Vector2 direction = (movementType == MovementType.Vertical) ? Vector2.up : Vector2.right;
        rb.velocity = (movingForward ? direction : -direction) * speed;

        // Перемещение игрока, если он на платформе
        if (playerOnPlatform != null)
        {
            Rigidbody2D playerRb = playerOnPlatform.GetComponent<Rigidbody2D>();
            Vector2 deltaMovement = rb.position - lastPosition;

            // Сохраняем горизонтальную скорость игрока (если он бежит)
            float playerInput = Input.GetAxis("Horizontal");
            float playerSpeed = playerOnPlatform.GetComponent<PlayerMovement>().moveSpeed;

            if (movementType == MovementType.Horizontal)
            {
                // Игрок может бежать в любом направлении независимо от платформы
                playerRb.velocity = new Vector2(
                    playerInput * playerSpeed + rb.velocity.x,
                    playerRb.velocity.y
                );
            }
            else
            {
                float currentXVelocity = playerInput * playerSpeed;
                float currentYVelocity = (playerRb.velocity.y > 0) ? playerRb.velocity.y : rb.velocity.y;

                // Для вертикальной платформы игрок сохраняет горизонтальную скорость
                playerRb.velocity = new Vector2(currentXVelocity, currentYVelocity);
            }
        }

        lastPosition = rb.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerMovement>().isGrounded())
        {
            playerOnPlatform = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}