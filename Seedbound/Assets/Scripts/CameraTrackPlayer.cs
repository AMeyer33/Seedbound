using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour
{
    [Header("Tracking Settings")]
    public Transform target; 
    public Vector3 offset = new Vector3(0, 0, -10f); 
    
    [Tooltip("Lower values = tighter tracking. 0.12f works well for top-down games.")]
    public float smoothTime = 0.12f; 

    private Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        // Syncs the game to the monitor's refresh rate to prevent frame-tearing and stutter
        QualitySettings.vSyncCount = 1; 

        // Caps frame rate to prevent the GPU from running uncapped in Editor/Build
        Application.targetFrameRate = 60; 
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            // SmoothDamp absorbs sudden direction flips without stuttering
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref currentVelocity, 
                smoothTime
            );
        }
    }
}