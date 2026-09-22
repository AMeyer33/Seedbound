using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour
{
    // Drag your player object into this slot in the Unity Inspector
    public Transform target; 
    
    // Adjust these values to position the camera relative to the player
    public Vector3 offset = new Vector3(0, 2, -10); 
    
    // Higher values mean tighter tracking; lower values mean smoother lag
    public float smoothTime = 0.25f; 
    
    private Vector3 currentVelocity = Vector3.zero;

    // Update runs every frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            
            // Smoothly dampens the camera movement
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }
}
