using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    public RawImage line;

    public void Update()
    {
        Music.Update();
    }

    public void OnClickGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickSound()
    {
        Music.GetInstance().SwitchSound(line);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
