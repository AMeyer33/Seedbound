using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class TopDownJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [Tooltip("Duration of the jump in seconds.")]
    public float jumpDuration = 0.6f;

    [Tooltip("Target scale multiplier at the peak of the jump.")]
    public float peakScaleMultiplier = 1.3f;

    [Header("Collision & Layers")]
    [Tooltip("The layer assigned to your Player object.")]
    public int playerLayer = 0; // Default layer index

    [Tooltip("The layer assigned to jumpable objects (e.g. Boxes).")]
    public int jumpableLayer = 0;

    private bool isJumping = false;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        
        // Auto-assign player layer if not set
        playerLayer = gameObject.layer;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isJumping)
        {
            StartCoroutine(PerformJump());
        }
    }

    private IEnumerator PerformJump()
    {
        isJumping = true;

        // 1. Instantly ignore collisions between the Player layer and Jumpable layer
        Physics2D.IgnoreLayerCollision(playerLayer, jumpableLayer, true);

        Vector3 peakScale = originalScale * peakScaleMultiplier;
        float halfDuration = jumpDuration / 2f;
        float elapsedTime = 0f;

        // Phase 1: Scale up
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, peakScale, t);
            yield return null;
        }

        elapsedTime = 0f;

        // Phase 2: Scale down
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / halfDuration;
            transform.localScale = Vector3.Lerp(peakScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;

        // 2. Re-enable collisions between Player and Jumpable layers on landing
        Physics2D.IgnoreLayerCollision(playerLayer, jumpableLayer, false);

        isJumping = false;
    }

    public bool IsJumping => isJumping;
}