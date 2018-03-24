using UnityEngine;

public class MainTower : MonoBehaviour {
    public int health = 10000;

    private void Update()
    {
        if (health <= 0 && transform.localScale.z > 0.1f)
            transform.localScale -= new Vector3(0.1f, 0.0f, 0.1f) * Time.deltaTime;
        health = Mathf.Max(health, 0);
    }
}
