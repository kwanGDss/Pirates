using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 3.0f;
    private float lastHorizonalInput = 0f;
    private bool isJumping = false;
    private int jumpCount = 0;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void FixedUpdate()
    {
        UpdateJumpState();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckLanding(collision);
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        UpdateCharacterDirection(horizontal);
    }

    private void UpdateCharacterDirection(float horizontal)
    {
        if (horizontal > 0)
        {
            lastHorizonalInput = 1f;
        }
        else if (horizontal < 0)
        {
            lastHorizonalInput = -1f;
        }

        if (lastHorizonalInput != 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * lastHorizonalInput;
            transform.localScale = newScale;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            isJumping = true;
            jumpCount++;
            if(jumpCount == 2)
            {
                animator.SetBool("IsDoubleJumping", true);
            }

            animator.SetBool("IsJumpingUp", true);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void UpdateJumpState()
    {
        if (!isJumping) return;

        if (rb.velocity.y < 0)
        {
            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsJumpingDown", true);
            animator.SetBool("IsDoubleJumping", false);
        }
    }

    private void CheckLanding(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground") return;

        isJumping = false;
        jumpCount = 0;
        animator.SetBool("IsJumpingDown", false);
        animator.SetBool("IsJumpingDown", false);
        animator.SetBool("IsDoubleJumping", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            animator.SetBool("IsDead", true);
        }   
    }

    private bool IsGrounded()
    {
        float extraHeightText = 0.1f;
        Vector2 rayStart = rb.position + new Vector2(0, -0.5f);
        Vector2 rayEnd = rayStart + Vector2.down * extraHeightText;

        Debug.DrawLine(rayStart, rayEnd, Color.red);

        RaycastHit2D raycastHit = Physics2D.Raycast(rayStart, Vector2.down, extraHeightText);
        return raycastHit.collider != null;
    }
}
