using UnityEngine;

public class UnitController : MonoBehaviour {
    public int health = 5000;
    public int fireInterval;
    public float attackRadius;
    public int damage;
    public GameObject projectilePrefab;

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
        if (health <= 0) {
            gameObject.GetComponent<Animator>().SetBool("Death", true);
            CurrentTowerDefenceState.GetInstance().ChangeBalance(100);
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
        gameObject.transform.LookAt(targetTower.transform);
        if (!hitted) {
            gameObject.GetComponent<Animator>().SetBool("Hit", true);
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
