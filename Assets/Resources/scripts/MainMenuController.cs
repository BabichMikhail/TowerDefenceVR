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

    public void OnClickContinue()
    {
        Time.timeScale = 1.0f;
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    public void OnClickSound()
    {
        Music.GetInstance().SwitchSound(line);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMenu()
    {
        Time.timeScale = 0.0f;
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
