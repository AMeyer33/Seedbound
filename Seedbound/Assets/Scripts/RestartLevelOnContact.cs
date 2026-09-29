using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class RestartLevelOnContact : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If checked, only objects with the specified tag will trigger a restart.")]
    public bool filterByTag = true;
    public string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryRestart(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryRestart(collision.gameObject);
    }

    private void TryRestart(GameObject hitObject)
    {
        // If filtering by tag, ensure the colliding object matches targetTag
        if (filterByTag && !hitObject.CompareTag(targetTag))
        {
            return;
        }

        // Reload current active scene
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}