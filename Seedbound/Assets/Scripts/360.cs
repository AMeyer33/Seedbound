using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 90f;

    [Tooltip("Check to rotate clockwise, uncheck for counter-clockwise.")]
    public bool clockwise = true;

    void Update()
    {
        // Determine rotation direction
        float direction = clockwise ? -1f : 1f;

        // Calculate degrees to rotate this frame
        float degreesPerFrame = rotationSpeed * direction * Time.deltaTime;

        // Apply rotation around the Z-axis (standard for 2D)
        transform.Rotate(0f, 0f, degreesPerFrame);
    }
}