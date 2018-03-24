using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int approximationDegree;
    public float speed;
    public float damageRadius;

    const float g = 9.81f;

    private Vector3 initialPoint;
    private Vector3 destinationPoint;
    private GameObject destinationObject;
    private int damage;
    private Vector3 speedVector;

    private int initialTime;

    private void Start()
    {
        initialTime = (int)(Time.time * 1000);
        if (approximationDegree == 2) {
            // speed = speed in projection on plane
            Vector3 route = (destinationPoint - initialPoint);
            route.y = 0;
            float flyTime = route.magnitude / speed;
           // float t2 = (initialPoint.y - destinationPoint.y) / (g * flyTime) + flyTime / 2f;
            float t1 = flyTime / 2f - (initialPoint.y - destinationPoint.y) / (g * flyTime);

            float alfa = Mathf.Atan((g*t1*t1/2)/(speed*t1));
            speedVector = new Vector3(
                route.x / route.magnitude * speed,
                g*t1,
                route.z / route.magnitude * speed
            );
            Debug.Log(speedVector);
        }
    }

    private void Update()
    {
        if (destinationObject == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 movement = new Vector3(0, 0, 0);

        Debug.Assert(initialPoint != null);
        Debug.Assert(destinationPoint != null);
        if (approximationDegree == 1) {
            movement = (destinationObject.transform.position - transform.position) * Time.deltaTime * speed;
            destinationPoint = destinationObject.transform.position;
        } else if (approximationDegree == 2) {
            float speedY = speedVector.y - g*Time.deltaTime;
            movement = new Vector3(speedVector.x, (speedY + speedVector.y) / 2, speedVector.z) * Time.deltaTime;
            speedVector.y = speedY;
            Debug.Log(speedVector);
        } else {
            Debug.Assert(false); 
        }

        transform.position += movement;

        if ((destinationPoint - transform.position).magnitude < 0.5) {
            if (approximationDegree == 1) {
                destinationObject.GetComponent<Unit>().health -= damage;
            }
            var unitCollection = CollectionContainer.unitCollection;
            for (int i = 0; i < unitCollection.transform.childCount; ++i) {
                var childObject = unitCollection.transform.GetChild(i).gameObject;
                if ((childObject.transform.position - destinationPoint).sqrMagnitude < damageRadius * damageRadius)
                    childObject.GetComponent<Unit>().health -= damage;
            }
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
