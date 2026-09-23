using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLawnMower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float speed = 2f;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 startingPosition;
    private bool movingRight = true;
    private float leftLimit;
    private float rightLimit;

    private void Start()
    {
        // Store starting position and calculate patrol boundaries
        startingPosition = transform.position;
        leftLimit = startingPosition.x - moveDistance;
        rightLimit = startingPosition.x + moveDistance;

        // Automatically get SpriteRenderer if not assigned in Inspector
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (movingRight)
        {
            // Move toward the right limit
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
                FlipSprite();
            }
        }
        else
        {
            // Move toward the left limit
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
                FlipSprite();
            }
        }
    }

    private void FlipSprite()
    {
        if (spriteRenderer != null)
        {
            // Flips the sprite horizontally using the SpriteRenderer component
            spriteRenderer.flipX = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Restart the current active scene when colliding with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Support for trigger collisions if your enemy or player uses triggers
        if (collider.CompareTag("Player"))
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
