using UnityEngine;

public class EnemyStopper2D : MonoBehaviour
{
    // Type the EXACT name of your tracking script here (e.g., EnemyMovement)
    // Replace 'YourTrackingScriptName' below with your actual script name.
    [SerializeField] private Follow trackingScript;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // If not manually assigned in Inspector, automatically search on this GameObject
        if (trackingScript == null)
        {
            trackingScript = GetComponent<Follow>();
        }
    }

    private void Update()
    {
        if (TouchTrigger2D.HasBeenTouched)
        {
            if (trackingScript != null && trackingScript.enabled)
            {
                trackingScript.enabled = false;
            }

            if (rb2d != null)
            {
                rb2d.linearVelocity = Vector2.zero;
            }

            enabled = false;
        }
    }
}