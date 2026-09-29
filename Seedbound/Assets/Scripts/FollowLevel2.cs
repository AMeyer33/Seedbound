using UnityEngine;

public class FollowLevel2 : MonoBehaviour
{
    public Transform objectToFollow;
    public float speed = 3f;

    [Header("Obstacle Avoidance")]
    public float checkDistance = 1f; // How far ahead/down to check for open space
    public LayerMask obstacleMask = ~0; // Set to Everything or your Obstacle layer

    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    void FixedUpdate()
    {
        if (objectToFollow == null)
            return;

        // Force Unity to update static colliders that moved without a Rigidbody
        Physics2D.SyncTransforms();

        Vector2 currentPos = rb.position;
        Vector2 targetPos = objectToFollow.position;

        // Stop moving when close enough to target
        if (Vector2.Distance(currentPos, targetPos) < 0.2f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 bestDirection = FindBestOpenDirection(currentPos, targetPos);
        rb.linearVelocity = bestDirection * speed;
    }

    private Vector2 FindBestOpenDirection(Vector2 currentPos, Vector2 targetPos)
    {
        Vector2 directDir = (targetPos - currentPos).normalized;

        // Size of the check area based on the follower's collider
        Vector2 checkSize = col != null ? col.bounds.size * 0.8f : new Vector2(0.4f, 0.4f);

        // Disable own collider temporarily so overlap checks don't hit itself
        if (col != null) col.enabled = false;

        // 1. Check if the direct path to objectToFollow is clear of colliders
        Collider2D directObstacle = Physics2D.OverlapBox(currentPos + (directDir * checkDistance), checkSize, 0f, obstacleMask);
        if (directObstacle == null || directObstacle.transform == objectToFollow)
        {
            if (col != null) col.enabled = true;
            return directDir; // Direct line to target is open!
        }

        // 2. Direct path is blocked! Evaluate 8 directions around the object to find open space
        Vector2[] testDirections = new Vector2[]
        {
            Vector2.down,
            (Vector2.down + Vector2.right).normalized,
            Vector2.right,
            (Vector2.up + Vector2.right).normalized,
            Vector2.up,
            (Vector2.up + Vector2.left).normalized,
            Vector2.left,
            (Vector2.down + Vector2.left).normalized
        };

        Vector2 bestDir = Vector2.zero;
        float highestScore = -1000f;

        foreach (Vector2 dir in testDirections)
        {
            Vector2 checkPosition = currentPos + (dir * checkDistance);
            Collider2D obstacle = Physics2D.OverlapBox(checkPosition, checkSize, 0f, obstacleMask);

            // If the space in this direction contains no obstacle colliders
            if (obstacle == null || obstacle.transform == objectToFollow)
            {
                // Score direction by how closely it points toward the target
                float score = Vector2.Dot(dir, directDir);

                if (score > highestScore)
                {
                    highestScore = score;
                    bestDir = dir;
                }
            }
        }

        if (col != null) col.enabled = true;

        return bestDir;
    }
}