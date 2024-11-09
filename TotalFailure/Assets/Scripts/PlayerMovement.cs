using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    Rigidbody2D rb;
    bool isFacingRight = false;

    [Header("Movement")]
    float moveSpeed = 5f;
    float horizontalInput;

    [Header("Jumping")]
    float jumpPower = 6f;
    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 2f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Dashing")]
    float dashSpeed = 15f;
    float dashDuration = 0.15f;
    float dashCooldown = 0.5f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontalInput));

        //Если сейчас ускорение, то ничего нельзя делать
        if (isDashing)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal"); //Лево или право

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        FlipSprite(); //Меняем местами спрайт

        Jump(); //Прыжок

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash()); //Ускорение
        }

        ChangeColliderWhenRush(); //Чтобы при беге не было вида, что он летает
    }

    private void ChangeColliderWhenRush()
    {
        if (isGrounded() && horizontalInput == 0) //Если стоит и не двигается
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, Vector2.down, 0.1f); //Ищем внизу платформу на расстоянии 0.1

            //Проверяем, есть ли платформа под игроком
            if (hit.collider != null)
            {
                float targetY = hit.point.y + 0.1f; // Платформа + небольшой отступ от неё
                rb.transform.position = new Vector2(rb.transform.position.x,  Mathf.Lerp(rb.transform.position.y, targetY, 0.001f)); //Переход от одного значения к другому за определенный период времени
            }
        }
    }

    private void Jump()
    {
        if (isGrounded()) //Если на земле
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }

        //Если 1 раз пробел то низко прыгнет, если зажать то выше
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true); //чтобы проходить через врагов
        canDash = false;
        isDashing = true;

        trailRenderer.emitting = true; //Анимация на ускорение
        float dashDirection = isFacingRight ? 1f : -1f;

        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y); //Ускоряем

        yield return new WaitForSeconds(dashDuration); //Ждём несколько секунд

        rb.velocity = new Vector2(0f, rb.velocity.y);

        isDashing = false;
        trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false); //чтобы проходить через врагов

        yield return new WaitForSeconds(dashCooldown); //Ещё ждём прежде чем снова сможем ускоряться
        canDash = true;
    }
}
