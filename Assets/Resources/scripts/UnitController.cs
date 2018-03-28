using UnityEngine;

public class UnitController : MonoBehaviour {
    public int health = 5000;
    public int fireInterval;
    public float attackRadius;
    public int damage;
    public GameObject projectilePrefab;

    private BaseRouter router;
    private GameObject targetTower;
    private float speed = 0.20f;
    private int lastShotTime = -10000;

    private bool canShootAtTower()
    {
        return (targetTower.GetComponentInChildren<BoxCollider>().ClosestPoint(transform.position) - transform.position).sqrMagnitude <= attackRadius * attackRadius;
    }

    public void Update()
    {
        if (health <= 0) {
            CurrentTowerDefenceState.GetInstance().ChangeBalance(100);
            Destroy(gameObject);
            return;
        }

        if (router.InPlace(transform.position, attackRadius) && canShootAtTower()) {
            if (lastShotTime + fireInterval < Time.time * 1000)
                ShootAtTower();
        } else {
            router.ApplyMovement(transform, Time.deltaTime, speed);
        }
    }

    private void ShootAtTower()
    {
        gameObject.transform.LookAt(targetTower.transform);
        var projectile = Instantiate(projectilePrefab, Container.GetInstance().GetProjectileContainer().transform);
        projectile.transform.position = gameObject.transform.position; // TODO real position
        projectile.GetComponent<ProjectileController>().SetUp(projectile.transform.position, targetTower.transform.position, targetTower, damage, ProjectileController.TargetType.TOWER);
        lastShotTime = (int)(Time.time * 1000);
    }

    public void SetUp(BaseRouter router, GameObject targetTower)
    {
        this.router = router;
        this.targetTower = targetTower;
    }
}
