using UnityEngine;

public class MainTowerController : MonoBehaviour {
    public int health = 10000;

    private void Update()
    {
        health = Mathf.Max(health, 0);
        if (health == 0 && transform.localScale.z > 0.1f)
            transform.localScale -= new Vector3(0.1f, 0.0f, 0.1f) * Time.deltaTime;
    }
}
