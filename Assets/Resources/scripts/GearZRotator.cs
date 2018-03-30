using UnityEngine;

public class GearZRotator : MonoBehaviour {
    public float speed = 1.0f;
    public float randomRotationSpeed;

    private void Start() {
        randomRotationSpeed = Random.Range(1.0f, 1.5f);
    }

    void Update () {
        transform.Rotate(new Vector3(0, 0, 4.0f * randomRotationSpeed * Time.deltaTime * speed));
    }
}
