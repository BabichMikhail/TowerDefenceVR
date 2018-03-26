using UnityEngine;

public class CanvasController : MonoBehaviour {
    void Update ()
    {
        var camera = Camera.current;
        if (camera != null) {
            transform.LookAt(camera.transform);
            var distance = (transform.TransformDirection(transform.position) - camera.transform.TransformDirection(camera.transform.position)).magnitude;
            transform.localScale = transform.localScale.normalized * distance * 1;
        }
	}
}
