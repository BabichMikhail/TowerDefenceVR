using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public void Update()
    {
        Music.Update();
    }

    public void OnClickGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
