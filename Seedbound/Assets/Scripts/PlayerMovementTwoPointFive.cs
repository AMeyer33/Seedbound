using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementTwoPointFive : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8.0f;

    [Tooltip("How fast the character turns/accelerates. 75 feels snappy while eliminating camera jitter.")]
    [SerializeField] private float acceleration = 75.0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Prevent rotation from physics collisions
        rb.freezeRotation = true;
        
        // Set gravity to 0 so the player doesn't fall down
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            // Horizontal Input (A / D or Left / Right Arrows)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal = 1f;
            }

            // Vertical Input (W / S or Up / Down Arrows)
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                vertical = -1f;
            }
            else if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                vertical = 1f;
            }
        }

        // Store vector normalized so diagonal movement isn't faster
        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;

        // Smoothly ramp velocity toward the target velocity across both axes
        Vector2 smoothedVelocity = Vector2.MoveTowards(
            rb.linearVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = smoothedVelocity;
    }
}