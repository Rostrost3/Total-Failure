using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    bool isFacingRight = false;

    [Header("Player Animation Settings")]
    public Animator animator;

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

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 1f;
    bool IsWallSliding;

    // bool isWallJumping;
    // float wallJumpDirection;
    // float wallJumpTime = 0.5f;
    // float wallJumpTimer;
    // public Vector2 wallJumpForce = new Vector2(5f, 6f);

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
        //���� ������ ���������, �� ������ ������ ������
        if (isDashing)
        {
            return;
        }

        animator.SetFloat("HorizontalMove", Mathf.Abs(horizontalInput)); // �������� ��� ��������

        horizontalInput = Input.GetAxis("Horizontal"); //���� ��� �����
        
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        FlipSprite(); //������ ������� ������

        Jump(); //������

        WallSlide(); //Скольжение по стене

        // WallJump(); //Прыжок от стены

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash()); //���������
        }
    }

    private void Jump()
    {
        if (isGrounded()) //���� �� �����
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }

        //���� 1 ��� ������ �� ����� �������, ���� ������ �� ����
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // //Прыжок от стены
        // if (wallJumpTimer > 0f)
        // {
        //     isWallJumping = true;
        //     rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
        //     wallJumpTimer = 0f;

        //     if (transform.localScale.x != wallJumpDirection)
        //     {
        //         isFacingRight = !isFacingRight;
        //         Vector3 ls = transform.localScale;
        //         ls.x *= -1f;
        //         transform.localScale = ls;
        //     }
            

        //     Invoke("CancelWallJump", wallJumpTime + 0.1f);
        // }
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

    private bool WallCheck()
    {
        if (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer))
        {
            return true;
        }
        return false;
    }

    
    private void WallSlide()
    {
        if (WallCheck() && !isGrounded() && horizontalInput != 0)
        {
            IsWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            IsWallSliding = false;
        }
    }

    // private void WallJump()
    // {
    //     if (IsWallSliding)
    //     {
    //         isWallJumping = false;
    //         wallJumpDirection = transform.localScale.x;
    //         wallJumpTimer = wallJumpTime;

    //         CancelInvoke("CancelWallJump");
    //     }
    //     else if (wallJumpTimer > 0f)
    //     {
    //         wallJumpTimer -= Time.deltaTime;
    //     }
    // }

    // private void CancelWallJump()
    // {
    //     isWallJumping = false;
    // }

    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true); //����� ��������� ����� ������
        canDash = false;
        isDashing = true;

        trailRenderer.emitting = true; //�������� �� ���������
        float dashDirection = isFacingRight ? 1f : -1f;

        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y); //��������

        yield return new WaitForSeconds(dashDuration); //��� ��������� ������

        rb.velocity = new Vector2(0f, rb.velocity.y);

        isDashing = false;
        trailRenderer.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false); //����� ��������� ����� ������

        yield return new WaitForSeconds(dashCooldown); //��� ��� ������ ��� ����� ������ ����������
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);

    }
}
