using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int damage = 1000;
    public int fireInterval = 2000;
    public float radius = 5f;

    private GameObject unitCollection;
    private GameObject projectileCollection;
    private int lastShotTime = -100000;
    private GameObject projectilePrefab;

    private void Start()
    {
        Debug.Assert(lastShotTime + fireInterval <= 0);
        unitCollection = CollectionContainer.unitCollection;
        projectileCollection = CollectionContainer.projectileCollection;
    }

    private void Update()
    {
        if (lastShotTime + fireInterval < Time.time * 1000) {
            Debug.Log("TRY FIRE");
            List<Transform> availableUnits = new List<Transform>();
            for (int i = 0; i < unitCollection.transform.childCount; ++i) {
                var child = unitCollection.transform.GetChild(i);
                //Debug.Log("Distance = " + (child.transform.position - gameObject.transform.position).magnitude);
                if ((child.transform.position - gameObject.transform.position).magnitude <= radius)
                    availableUnits.Add(child);
            }

            if (availableUnits.Count > 0)
                ShootAt(availableUnits[Random.Range(0, availableUnits.Count)]);
        }
    }

    private void ShootAt(Transform unit)
    {
        gameObject.transform.LookAt(unit); // TODO animation, rotate speed
        var projectile = Instantiate(projectilePrefab, projectileCollection.transform);
        projectile.transform.position = gameObject.transform.position; // TODO real projectile execute position
        projectile.GetComponent<Projectile>().SetUp(projectile.transform.position, unit.transform.position, unit.gameObject, damage);
        lastShotTime = (int)(Time.time * 1000);
    }

    public void SetProjectile(GameObject projectile)
    {
        projectilePrefab = projectile;
    }
}
