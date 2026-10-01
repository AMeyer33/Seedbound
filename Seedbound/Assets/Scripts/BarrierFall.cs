using UnityEngine;

public class BarrierFall : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private GameObject barrier;

    [Header("Movement Settings")]
    [SerializeField] private Vector3 loweredPositionOffset = new Vector3(0, -100f, 0); // Distance the barrier falls
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 upPosition;
    private Vector3 downPosition;
    private int objectsOnButton = 0; // Tracks how many valid objects are pressing the button

    private void Start()
    {
        // Calculate the starting position and the target dropped position
        upPosition = barrier.transform.position;
        downPosition = upPosition + loweredPositionOffset;
    }

    private void Update()
    {
        // Smoothly slide the barrier between its up and down states
        Vector3 targetPosition = (objectsOnButton > 0) ? downPosition : upPosition;
        barrier.transform.position = Vector3.MoveTowards(barrier.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering is a Player or a Box
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnButton++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reduce the count when they step off
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnButton = Mathf.Max(0, objectsOnButton - 1); // Prevents going below 0
        }
    }
}