using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    public enum MovementType { Vertical, Horizontal }
    public MovementType movementType = MovementType.Vertical; // ��� ��������

    public float moveDistance = 7f; // ��������� ��������
    public float speed = 3f; // �������� ��������

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
            rb = gameObject.AddComponent<Rigidbody2D>(); // ��������� Rigidbody2D, ���� ��� ���
        }

        rb.bodyType = RigidbodyType2D.Kinematic; // ������ ��������� ��������������
        rb.gravityScale = 0; // ��������� ����������
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // �������� ��������� ��������
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

        rb.velocity = targetVelocity; // ������� ��������� ����� velocity

        // ���� �������� �� ���������, ������� ��� ������ � ����������
        if (player != null)
        {
            // ��������� ������ ��������� ��� ��� �������� ����� ���������
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 platformVelocity = rb.velocity;

            // ���� ��������� ��������� ������������ ���������
            if (movementType == MovementType.Horizontal)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                playerRb.velocity = new Vector2(horizontalInput * speed + platformVelocity.x, playerRb.velocity.y);
            }
            else
            {
                // ���� ������������ �������� ���������
                playerRb.velocity = new Vector2(playerRb.velocity.x, platformVelocity.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // ���������� ���������
            playerOffset = player.position - transform.position; // ��������� �������� ������������ ���������
            player.SetParent(transform); // ����������� ��������� � ���������
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetParent(null); // ���������� ���������
            player = null;
        }
    }
}
