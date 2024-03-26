using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load scene using name, or reload the active scene
/// </summary>
public class LoadScene : MonoBehaviour
{
    public void LoadSceneUsingName(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
