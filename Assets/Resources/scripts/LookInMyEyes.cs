using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInMyEyes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var camera = Camera.current;
        if (camera != null)
        {
            transform.LookAt(camera.transform);
            var distance = (transform.TransformDirection(transform.position) - camera.transform.TransformDirection(camera.transform.position)).magnitude;
            var scale = transform.localScale;
            scale.Normalize();
            transform.localScale = scale * distance * 1;
        }
	}
}
