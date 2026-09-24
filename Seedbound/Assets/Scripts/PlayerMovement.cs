using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private float horizontalInput;

    [Header("Jumping")]
    public float jumpForce = 12f;

    // Gravity while moving upward and holding jump
    public float jumpGravityMultiplier = 1.5f;

    // Gravity while falling
    public float fallMultiplier = 3f;

    // Gravity when jump is released early
    public float lowJumpMultiplier = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool jumpRequested;
    private bool isHoldingJump;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }

        // Horizontal input
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput = -1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput = 1f;
            }

            // Jump input
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpRequested = true;
            }

            isHoldingJump = Keyboard.current.spaceKey.isPressed;
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );

        // Jump
        if (jumpRequested)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );
            }

            jumpRequested = false;
        }

        ApplyVariableGravity();
    }

    private void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            if (!isHoldingJump)
            {
                // Released jump early
                rb.linearVelocity += Vector2.up *
                    Physics2D.gravity.y *
                    (lowJumpMultiplier - 1) *
                    Time.fixedDeltaTime;
            }
            else
            {
                // Holding jump: still apply extra gravity
                // to make the initial jump feel snappy
                rb.linearVelocity += Vector2.up *
                    Physics2D.gravity.y *
                    (jumpGravityMultiplier - 1) *
                    Time.fixedDeltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}
