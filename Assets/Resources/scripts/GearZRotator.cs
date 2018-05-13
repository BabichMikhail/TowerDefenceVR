using UnityEngine;

public class GearZRotator : MonoBehaviour {
    public float randomRotationSpeed;

    private void Start()
    {
        randomRotationSpeed = Random.Range(1.5f, 2.5f) * (Random.value > 0.5 ? 1.0f : -1.0f);
    }

    private void Update ()
    {
        transform.Rotate(new Vector3(0, 0, 4.0f * randomRotationSpeed * Time.deltaTime));
    }
}
