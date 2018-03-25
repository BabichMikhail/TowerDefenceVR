using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {
    public int health = 5000;
    public int fireInterval;
    public float attackRadius;
    public int damage;
    public GameObject projectilePrefab;

    private BaseRouter router;
    private GameObject targetTower;
    private float speed = 0.20f;
    private int lastShotTime = -10000;

    public void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
            CurrentTowerDefenceState.GetInstance().ChangeBalance(100);
        }

        if (router.InPlace(transform.position, attackRadius)){
            if (lastShotTime + fireInterval < Time.time * 1000) {
                var unitCollection = CollectionContainer.unitCollection;
                List<Transform> availableUnits = new List<Transform>();
                if ((targetTower.transform.position - transform.position).magnitude <= attackRadius)
                    ShootAtTower();
            }
        } else {
            router.ApplyMovement(transform, Time.deltaTime, speed);
        }
    }

    private void ShootAtTower()
    {
        gameObject.transform.LookAt(targetTower.transform);
        var projectile = Instantiate(projectilePrefab, CollectionContainer.projectileCollection.transform);
        projectile.transform.position = gameObject.transform.position; // TODO real position
        projectile.GetComponent<Projectile>().SetUp(projectile.transform.position, targetTower.transform.position, targetTower, damage, Projectile.TargetType.TOWER);
        lastShotTime = (int)(Time.time * 1000);
    }

    public void SetUp(BaseRouter router, GameObject targetTower)
    {
        this.router = router;
        this.targetTower = targetTower;
    }
}
