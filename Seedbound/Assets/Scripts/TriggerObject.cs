using UnityEngine;

public class TouchTrigger2D : MonoBehaviour
{
    public static bool HasBeenTouched { get; private set; } = false;

    private void Awake()
    {
        // Reset the flag every time the scene starts or loads
        HasBeenTouched = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HasBeenTouched = true;
            Debug.Log("Object touched! Enemy stopped.");
        }
    }
}