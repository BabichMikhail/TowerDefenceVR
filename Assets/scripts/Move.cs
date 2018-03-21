using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public BaseRouter router;

    public void Update()
    {
        if (router.InPlace())
            return; // TODO remove object
        var movement = Time.deltaTime * router.GetMovement(transform);
        transform.position = transform.position + (new Vector3(movement.x, 0, movement.y));
    }
}
