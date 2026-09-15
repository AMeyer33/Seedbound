using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void GotoSceneLevelSeclect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    
}
