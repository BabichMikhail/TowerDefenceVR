using System.Collections.Generic;
using UnityEngine;

public class CollectionContainer {
    public static GameObject unitCollection;
    public static GameObject projectileCollection;
    public static GameObject routeCollection;
    public static GameObject towerCollection;
}

public class Main : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;
    public GameObject archedTower;
    public GameObject archedProjectile;
    public GameObject siegeTower;
    public GameObject siegeProjectile;
    public Canvas createTowerCanvas;
    public Canvas changeTowerCanvas;

    public GameObject unitCollection;
    public GameObject projectileCollection;
    public GameObject routeCollection;
    public GameObject towerCollection;

    private void Awake()
    {
        CollectionContainer.routeCollection = routeCollection;
        CollectionContainer.towerCollection = towerCollection;
        CollectionContainer.unitCollection = unitCollection;
        CollectionContainer.projectileCollection = projectileCollection;
    }

    private void Start()
    {
        var mainTowerObject = GameObject.FindGameObjectWithTag("MainTower");
        var finalPoint = BaseRouter.GetObjectPosition(mainTowerObject.transform);

        foreach (var routerObject in GameObject.FindGameObjectsWithTag("Router"))
        {
            var router = new SampleRouter();
            router.points = new List<Vector2>();
            for (int i = 0; i < routerObject.transform.childCount; ++i)
                router.points.Add(BaseRouter.GetObjectPosition(routerObject.transform.GetChild(i).transform));
            router.points.Add(finalPoint);
            routers.Add(router);
        }

        var state = CurrentTowerDefenceState.GetInstance();
        state.SetCanvases(createTowerCanvas, changeTowerCanvas);
    }

    public void SendUnit()
    {
        var unit = Instantiate(units[Random.Range(0, units.Length)], unitCollection.transform);
        var router = routers[Random.Range(0, routers.Count)];
        unit.GetComponent<Unit>().router = router.CopyInstance();
        var initialPoint = router.GetInitialPoint();
        unit.transform.position = new Vector3(initialPoint.x, 0, initialPoint.y);
    }

    public void OnMouseUp()
    {
        SendUnit();
    }

    public void createArchedTower()
    {
        Debug.Log("Arched tower");
        CurrentTowerDefenceState.GetInstance().CreateTower(archedTower, archedProjectile);
    }

    public void createSiegeTower()
    {
        Debug.Log("Siege tower");
        CurrentTowerDefenceState.GetInstance().CreateTower(siegeTower, siegeProjectile);
    }

    public void increaseTowerSpeed()
    {
        Debug.Log("Tower speed");
        CurrentTowerDefenceState.GetInstance().UpdgradeCurrentTower(CurrentTowerDefenceState.UpgradeTypes.UPDRADE_SPEED, 1);
    }

    public void increaseTowerDamage()
    {
        Debug.Log("Tower damage");
        CurrentTowerDefenceState.GetInstance().UpdgradeCurrentTower(CurrentTowerDefenceState.UpgradeTypes.UPGRADE_DAMAGE, 1);
    }
}
