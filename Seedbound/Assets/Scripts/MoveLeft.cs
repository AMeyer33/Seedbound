using UnityEngine;
using System.Collections.Generic;

public class TriggerMultipleRemoteMove : MonoBehaviour
{
    [Header("Targets to Move")]
    [Tooltip("Drag all objects you want to move into this list")]
    public List<GameObject> objectsToMove;

    [Header("Movement Settings")]
    [Tooltip("Distance to move to the left in Unity units")]
    public float moveDistance = 3.0f;

    [Tooltip("Time in seconds to complete the move")]
    public float duration = 1.0f;

    [Header("Detection Settings")]
    [Tooltip("Tag assigned to your Player GameObject")]
    public string playerTag = "Player";

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !hasBeenTriggered)
        {
            hasBeenTriggered = true;

            // Start moving every object in the list
            foreach (GameObject obj in objectsToMove)
            {
                if (obj != null)
                {
                    StartCoroutine(MoveLeftRoutine(obj.transform));
                }
            }
        }
    }

    private System.Collections.IEnumerator MoveLeftRoutine(Transform targetTransform)
    {
        Vector3 startPosition = targetTransform.position;
        Vector3 targetPosition = startPosition + (Vector3.left * moveDistance);
        
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.position = targetPosition;
    }
}