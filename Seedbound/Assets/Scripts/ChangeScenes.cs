using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void GotoSceneLevelSeclect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GotoSceneLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GotoSceneLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    
    public void GotoSceneLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    
    public void GotoSceneLevel4()
    {
        SceneManager.LoadScene("Level4");
    }
    
    public void GotoSceneLevel5()
    {
        SceneManager.LoadScene("Level5");
    }
    
}
