using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int approximationDegree;
    public float speed = 1;

    private Vector3 initialPoint;
    private Vector3 destinationPoint;
    private GameObject destinationObject;
    private int damage;

    private void Update()
    {
        if (destinationObject == null) {
            Destroy(gameObject);
            return;
        }

        Debug.Assert(initialPoint != null);
        Debug.Assert(destinationPoint != null);
        if (approximationDegree == 1) {
            transform.position += (destinationObject.transform.position - transform.position) * Time.deltaTime * speed;
            destinationPoint = destinationObject.transform.position;
        } else if (approximationDegree == 2) {
            Debug.Assert(false, "Not implemented");
        } else {
            Debug.Assert(false); 
        }

        if ((destinationPoint - transform.position).magnitude < 0.5) {
            destinationObject.GetComponent<Unit>().health -= damage;
            Destroy(gameObject);
        }
    }

    public void SetUp(Vector3 newInitialPoint, Vector3 newDestinationPoint, GameObject newDestinationObject, int newDamage)
    {
        initialPoint = newInitialPoint;
        destinationPoint = newDestinationPoint;
        destinationObject = newDestinationObject;
        damage = newDamage;
    }
}
