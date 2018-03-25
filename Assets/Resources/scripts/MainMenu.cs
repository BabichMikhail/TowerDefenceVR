using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void OnClickGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
