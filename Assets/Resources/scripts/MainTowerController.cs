using UnityEngine;
using UnityEngine.UI;

public class MainTowerController : MonoBehaviour {
    public int health = Config.MAIN_TOWER_HEALTH;
    public GameObject defeatMenu;
    public GameObject gameMenu;

    public GameObject towerHealthText;

    private void Update()
    {
        health = Mathf.Max(health, 0);
        towerHealthText.GetComponent<Text>().text = (health / 10).ToString();
        if (health == 0) {
            gameMenu.GetComponent<Canvas>().enabled = false;
            defeatMenu.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0.0f;
        }
    }
}
