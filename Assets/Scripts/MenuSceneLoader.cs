using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        Genesis.Clear();
        SceneManager.LoadScene(1);
    }

    public void LoadTitle()
    {
        Genesis.Clear();
        SceneManager.LoadScene(0);
    }
}
