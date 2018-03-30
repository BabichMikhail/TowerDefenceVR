using UnityEngine;

public class MainTowerController : MonoBehaviour {
    public int health = 10000;

    private void Update()
    {
        health = Mathf.Max(health, 0);
        if (health == 0)
            Debug.Log("FAIL");
    }
}
