using UnityEngine;

public class UnitController : MonoBehaviour {
    public int health = 5000;
    public int fireInterval;
    public float attackRadius;
    public int damage;
    public int type;
    public GameObject projectilePrefab;
    public bool fallenAfterDeath;
    public Vector3 acceleration;

    public const int UNIT_TYPE_FLYING = 0;
    public const int UNIT_TYPE_PEDESTRIAN = 1;

    private BaseRouter router;
    private GameObject targetTower;
    private float speed = 3.0f;
    private int lastShotTime = -10000;
    private bool disabled = false;
    private bool hitted = false;
    private Vector3 deathSpeed = new Vector3(0.0f, 0.0f, 0.0f);

    private bool canShootAtTower()
    {
        return (targetTower.GetComponentInChildren<BoxCollider>().ClosestPoint(transform.position) - transform.position).sqrMagnitude <= attackRadius * attackRadius;
    }

    public void Update()
    {
        if (disabled) {
            Animation.TryAnimate(gameObject, "AfterDeath");
            if (fallenAfterDeath) {
                transform.Translate(deathSpeed);
                deathSpeed += acceleration * Time.deltaTime;
            }
            return;
        }

        if (health <= 0) {
            router.Stop(transform);
            Animation.TryAnimate(gameObject, "Death");
            CurrentTowerDefenceState.Instance.ChangeBalance(10);
            Destroy(gameObject, 6);
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
        if (!hitted) {
            Animation.TryAnimate(gameObject, "Hit");
            hitted = true;
        }
        var projectile = Instantiate(projectilePrefab, Container.Instance.ProjectileContainer.transform);
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
