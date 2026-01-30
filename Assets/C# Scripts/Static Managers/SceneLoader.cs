using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}