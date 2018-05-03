using UnityEngine;

public class UnitController : MonoBehaviour {
    public int health = 5000;
    public int fireInterval;
    public float attackRadius;
    public int damage;
    public int type;
    public GameObject projectilePrefab;
    public bool fallenAfterDeath;

    public const int UNIT_TYPE_FLYING = 0;
    public const int UNIT_TYPE_PEDESTRIAN = 1;

    private BaseRouter router;
    private GameObject targetTower;
    private float speed = 3.0f;
    private int lastShotTime = -10000;
    private bool disabled = false;
    private bool hitted = false;

    private bool canShootAtTower()
    {
        return (targetTower.GetComponentInChildren<BoxCollider>().ClosestPoint(transform.position) - transform.position).sqrMagnitude <= attackRadius * attackRadius;
    }

    public void Update()
    {
        if (disabled)
            return;
        // TODO remove mixed router
        if (health <= 0) {
            router.Stop(transform);
            Animation.TryAnimate(gameObject, "Death");
            Animation.TryAnimate(gameObject, "AfterDeath");
            if (fallenAfterDeath)
                transform.localPosition = transform.localPosition - new Vector3(0, -0.1f, 0);
            CurrentTowerDefenceState.GetInstance().ChangeBalance(10);
            Destroy(gameObject, 4);
            disabled = true;
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
        gameObject.transform.LookAt(targetTower.transform); // TODO without look at or look at in 2D
        if (!hitted) {
            Animation.TryAnimate(gameObject, "Hit");
            hitted = true;
        }
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
