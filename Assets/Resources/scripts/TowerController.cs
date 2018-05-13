using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {
    public int damage = 1000;
    public int fireInterval = 2000;
    public float radius = 20f;
    public Vector3 firePoint = new Vector3(0, 0, 0);
    public GameObject projectilePrefab;
    public int[] unitTypesForAttack;

    private int lastShotTime = -100000;

    private void Start()
    {
        Debug.Assert(lastShotTime + fireInterval <= 0);
    }

    private void Update()
    {
        Dictionary<int, bool> typesSet = new Dictionary<int, bool>();
        for (int i = 0; i < unitTypesForAttack.Length; ++i)
            typesSet[unitTypesForAttack[i]] = true;
        if (lastShotTime + fireInterval < Time.time * 1000) {
            var availableUnits = new List<Transform>();
            var unitContainer = Container.Instance.UnitContainer;
            var shootAtCenterPoint = transform.parent.GetComponent<TowerPositionController>().GetShootAtCenterPoint();
            for (int i = 0; i < unitContainer.transform.childCount; ++i) {
                var child = unitContainer.transform.GetChild(i);
                if ((child.transform.position - shootAtCenterPoint).magnitude <= radius && typesSet.ContainsKey(child.gameObject.GetComponent<UnitController>().type))
                    availableUnits.Add(child);
            }

            if (availableUnits.Count > 0)
                ShootAt(availableUnits[Random.Range(0, availableUnits.Count)]);
        }
    }

    private void ShootAt(Transform unit)
    {
        var projectile = Instantiate(projectilePrefab, Container.Instance.ProjectileContainer.transform);
        transform.localPosition += firePoint;
        projectile.transform.position = transform.position;
        transform.localPosition -= firePoint;
        projectile.GetComponent<ProjectileController>().SetUp(projectile.transform.position, unit.transform.position, unit.gameObject, damage, ProjectileController.TargetType.UNIT);
        lastShotTime = (int)(Time.time * 1000);
    }

    public void SetProjectile(GameObject projectile)
    {
        projectilePrefab = projectile;
    }
}
