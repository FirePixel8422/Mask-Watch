using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader
{
    [SerializeField] private string sceneName;

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}