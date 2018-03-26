using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int damage = 1000;
    public int fireInterval = 2000;
    public float radius = 5f;

    private int lastShotTime = -100000;
    private GameObject projectilePrefab;

    private void Start()
    {
        Debug.Assert(lastShotTime + fireInterval <= 0);
    }

    private void Update()
    {
        if (lastShotTime + fireInterval < Time.time * 1000) {
            var availableUnits = new List<Transform>();
            var unitContainer = Container.GetInstance().GetUnitContainer();
            for (int i = 0; i < unitContainer.transform.childCount; ++i) {
                var child = unitContainer.transform.GetChild(i);
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
        var projectile = Instantiate(projectilePrefab, Container.GetInstance().GetProjectileContainer().transform);
        projectile.transform.position = gameObject.transform.position; // TODO real position
        projectile.GetComponent<ProjectileController>().SetUp(projectile.transform.position, unit.transform.position, unit.gameObject, damage, ProjectileController.TargetType.UNIT);
        lastShotTime = (int)(Time.time * 1000);
    }

    public void SetProjectile(GameObject projectile)
    {
        projectilePrefab = projectile;
    }
}
