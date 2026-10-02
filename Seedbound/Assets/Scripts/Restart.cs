using UnityEngine;
using UnityEngine.SceneManagement; // Required for managing scenes

public class Restart : MonoBehaviour
{
    public void RestartLevel()
    {
        // Resets the time scale to normal in case your game was paused
        Time.timeScale = 1f; 
        
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
