using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class Follow : MonoBehaviour
{
    public Transform objectToFollow;
    public float speed = 3f;

    void Update()
    {
        if (objectToFollow == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            objectToFollow.position,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the touched object is the target player
        if (objectToFollow != null && other.transform == objectToFollow)
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        // Reloads the currently active scene
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}