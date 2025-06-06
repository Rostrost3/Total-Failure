using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    bool isFacingRight = false;
    public bool isDialogueActive = false;


    [Header("Player Animation Settings")]
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 7f;
    float horizontalInput;
    

    [Header("Jumping")]
    float jumpPower = 7f;
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
    public LayerMask EnemyLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 1f;
    bool IsWallSliding;

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.4f;
    float wallJumpTimer;
    public Vector2 wallJumpForce = new Vector2(5f, 6f);

    [Header("Dashing")]
    float dashSpeed = 15f;
    float dashDuration = 0.25f; //0.15f
    float dashCooldown = 0.5f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        int continueGame = PlayerPrefs.GetInt("ContinueGame", 0);
        if (continueGame == 1)
        {
            Vector2 position = SaveSystem.LoadPosition();
            if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "PlayerX") && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "PlayerY"))
            {
                transform.position = position;
            }
        }
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

        animator.SetBool("Jump", Input.GetButtonDown("Jump"));

        animator.SetBool("WallSlide", IsWallSliding);

        animator.SetBool("Attack", Input.GetMouseButtonDown(0));
        if (!isDialogueActive) { horizontalInput = Input.GetAxis("Horizontal"); }
        else { horizontalInput = 0f; }
          //���� ��� �����
        
        if (!isWallJumping)
        {   
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            FlipSprite(); //������ ������� ������
        }
        


        Jump(); //������

        WallSlide(); //Скольжение по стене

        WallJump(); //Прыжок от стены

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDialogueActive)
        {
            StartCoroutine(Dash()); //���������
        }
    }

    private void Jump()
    {
        if (isGrounded() && !isDialogueActive) //���� �� �����
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

        //Прыжок от стены
        if (Input.GetButtonDown("Jump") && (wallJumpTimer > 0f))
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y);
            wallJumpTimer = 0f;

            if (isFacingRight && wallJumpDirection < 0f || !isFacingRight && wallJumpDirection > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
            

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
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

    public bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    private bool WallCheck()
    {
        if (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer) && !Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, EnemyLayer))
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

    private void WallJump()
    {
        if (IsWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

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