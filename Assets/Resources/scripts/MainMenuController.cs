using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    public RawImage line;

    public void Awake()
    {
        Music.Instance = new Music();
    }

    public void Update()
    {
        Music.Instance.Update();
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
        Music.Instance.SwitchSound(line);
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

    public void OnClickRestart()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadScene("Main");
        Time.timeScale = 1.0f;
    }
}
