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
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}
