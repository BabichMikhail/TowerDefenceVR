using UnityEngine;

public class MainTowerController : MonoBehaviour {
    public int health = 10000;
    public GameObject defeatMenu;
    public GameObject gameMenu;

    private void Update()
    {
        health = Mathf.Max(health, 0);
        if (health == 0) {
            gameMenu.GetComponent<Canvas>().enabled = false;
            defeatMenu.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0.0f;
        }
    }
}
