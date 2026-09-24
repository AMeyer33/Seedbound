using UnityEngine;
using UnityEngine.SceneManagement;

public class MoleMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkDistance = 3f;
    public float moveSpeed = 2f;

    [Header("Holes")]
    public Transform[] holes;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private int currentHole = 0;
    private bool walkingOut = true;

    void Start()
    {
        // Make sure at least one hole is assigned
        if (holes.Length == 0)
        {
            Debug.LogError("Mole: No holes have been assigned!");
            return;
        }

        // Start at the first hole
        transform.position = holes[0].position;

        startPosition = transform.position;

        // Walk LEFT from the hole
        targetPosition = new Vector3(
            startPosition.x - walkDistance,
            startPosition.y,
            startPosition.z
        );

        // Face LEFT
        spriteRenderer.flipX = false;
    }

    void Update()
    {
        // Move toward the target
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (walkingOut)
            {
                // We reached the end of the path
                // Turn around and walk back
                walkingOut = false;

                targetPosition = startPosition;

                // Face RIGHT
                spriteRenderer.flipX = true;
            }
            else
            {
                // We reached the hole
                TeleportToNewHole();
            }
        }
    }

    void TeleportToNewHole()
    {
        // Pick a random hole
        int newHole = Random.Range(0, holes.Length);

        // Make sure we don't pick the same hole
        while (newHole == currentHole && holes.Length > 1)
        {
            newHole = Random.Range(0, holes.Length);
        }

        currentHole = newHole;

        // Teleport to the new hole
        transform.position = holes[currentHole].position;

        // Set the new starting position
        startPosition = transform.position;

        // Walk LEFT from the new hole
        targetPosition = new Vector3(
            startPosition.x - walkDistance,
            startPosition.y,
            startPosition.z
        );

        // Face LEFT
        spriteRenderer.flipX = false;

        walkingOut = true;
    }

    // Player touches the mole
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restart the current level
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }
}