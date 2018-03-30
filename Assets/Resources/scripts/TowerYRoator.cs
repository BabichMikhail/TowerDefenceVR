using UnityEngine;

public class TowerYRoator : MonoBehaviour {
    public float ratateSpeed = 1.0f;

    private int rotateSign = 1;
    private int verticalSign = 1;
    private Vector3 initialPosition;
    private int time = 0;

    private void Start()
    {
        transform.Rotate(new Vector3(0, Random.Range(0.0f, 360.0f), 0));
        ratateSpeed = Random.Range(1.0f, 10.0f);
        rotateSign = Random.Range(0, 2) == 0 ? 1 : -1;
        initialPosition = transform.position;
    }

    void Update () {
        transform.Rotate(new Vector3(0, rotateSign *  Time.deltaTime * ratateSpeed, 0));
        transform.Translate(new Vector3(0, verticalSign * Time.deltaTime, 0));
        time += (int)(1000 * Time.deltaTime);
        if (time > 5000) {
            time -= 5000;
            verticalSign *= -1;
        }
    }
}
