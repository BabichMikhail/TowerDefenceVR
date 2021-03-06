﻿using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public int approximationDegree;
    public float speed;
    public float damageRadius;

    const float g = 9.81f / 2;

    private Vector3 initialPoint;
    private Vector3 destinationPoint;
    private GameObject destinationObject;
    private int damage;
    private Vector3 speedVector;

    public enum TargetType { TOWER, UNIT }
    private TargetType targetType;

    private void Awake()
    {
        var audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.time = Config.AUDIO_SOURCE_PROJECTILE_SOUND_TIME;
    }

    private void Start()
    {
        if (approximationDegree == 2) {
            // TODO speed = speed in projection on plane
            Vector3 route = (destinationPoint - initialPoint);
            route.y = 0;
            float flyTime = route.magnitude / speed;
            float t1 = flyTime / 2f - (initialPoint.y - destinationPoint.y) / (g * flyTime);

            speedVector = new Vector3(
                route.x / route.magnitude * speed,
                g * t1,
                route.z / route.magnitude * speed
            );
        }

        transform.LookAt(destinationObject.transform);
    }

    private void Update()
    {
        if (destinationObject == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 movement = new Vector3(0, 0, 0);

        if (approximationDegree == 1) {
            movement = (destinationObject.transform.position - transform.position).normalized * Time.deltaTime * speed;
            destinationPoint = destinationObject.transform.position;
        } else if (approximationDegree == 2) {
            float speedY = speedVector.y - g*Time.deltaTime;
            movement = new Vector3(speedVector.x, (speedY + speedVector.y) / 2, speedVector.z) * Time.deltaTime;
            speedVector.y = speedY;
        } else {
            Debug.Assert(false); 
        }

        transform.position += movement;

        if ((destinationPoint - transform.position).magnitude < 0.5) {
            if (approximationDegree == 1) {
                if (targetType == TargetType.UNIT)
                    destinationObject.GetComponent<UnitController>().health -= damage;
                else
                    destinationObject.GetComponent<MainTowerController>().health -= damage;
            }

            if (targetType == TargetType.UNIT) {
                var unitCollection = Container.Instance.UnitContainer;
                for (int i = 0; i < unitCollection.transform.childCount; ++i) {
                    var childObject = unitCollection.transform.GetChild(i).gameObject;
                    if ((childObject.transform.position - destinationPoint).sqrMagnitude < damageRadius * damageRadius)
                        childObject.GetComponent<UnitController>().health -= damage;
                }
            }
            Destroy(gameObject);
        }
    }

    public void SetUp(Vector3 newInitialPoint, Vector3 newDestinationPoint, GameObject newDestinationObject, int newDamage, TargetType newTargetType)
    {
        initialPoint = newInitialPoint;
        destinationPoint = newDestinationPoint;
        destinationObject = newDestinationObject;
        damage = newDamage;
        targetType = newTargetType;
    }
}
