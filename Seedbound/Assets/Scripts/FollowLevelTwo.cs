using UnityEngine;

public class FollowLevelTwo : MonoBehaviour
{
    public Transform objectToFollow;
    public float speed = 3f;
    public float stopDistance = 0.2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        if (objectToFollow == null) return;

        // Stop moving if close enough to the flower
        float distance = Vector2.Distance(rb.position, objectToFollow.position);
        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Calculate direction toward target
        Vector2 direction = ((Vector2)objectToFollow.position - rb.position).normalized;

        // Move using Rigidbody velocity
        rb.linearVelocity = direction * speed;
    }
}