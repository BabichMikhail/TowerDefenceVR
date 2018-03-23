using UnityEngine;

public class Unit : MonoBehaviour {
    public BaseRouter router;
    public int health = 5000;

    public void Update()
    {
        if (router.InPlace())
            return; // TODO remove object
        var movement = Time.deltaTime * router.GetMovement(transform);
        transform.position = transform.position + (new Vector3(movement.x, 0, movement.y));
    }
}
