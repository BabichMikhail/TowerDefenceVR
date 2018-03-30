using UnityEngine;

public class GearZRotator : MonoBehaviour {
    public float speed = 1.0f;

    void Update () {
        transform.Rotate(new Vector3(0, 0, 1.0f * Time.deltaTime * speed));
    }
}
