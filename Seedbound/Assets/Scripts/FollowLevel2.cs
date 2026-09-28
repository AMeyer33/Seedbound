using UnityEngine;

public class BeeFollow : MonoBehaviour
{
    public Transform flower;
    public float speed = 3f;

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
        if (flower == null)
            return;

        Vector2 direction = ((Vector2)flower.position - rb.position).normalized;

        rb.linearVelocity = direction * speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Stop when the bee is touching an obstacle
        if (collision.gameObject != flower.gameObject)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}