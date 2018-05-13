using System.Collections.Generic;
using UnityEngine;

public class MyIntTuple
{
    public int first;
    public int second;
    public int third;

    public MyIntTuple(int first, int second, int third) { this.first = first; this.second = second; this.third = third; }
}

public class MainController : MonoBehaviour {
    public GameObject mainTower;
    public GameObject[] towers0;
    public GameObject[] towers1;

    private List<RouterController> routerControllers = new List<RouterController>();
    private float startedAt;
    private float lastIncreaseMoneyTime;
    private int unitWaveIndex = 0;
    private int lastSendUnitTime = -1000;

    // time in milliseconds, unit count, respawn index
    private List<MyIntTuple> UNIT_WAVES = new List<MyIntTuple>() {
        new MyIntTuple(1, 1, 2),
        new MyIntTuple(3000, 2, 0),
        new MyIntTuple(23000, 1, 1),
        new MyIntTuple(24000, 1, 2),
        new MyIntTuple(30000, 1, 2),
        new MyIntTuple(40000, 2, 0),
        new MyIntTuple(43000, 3, 1),
        new MyIntTuple(43000, 1, 2),
        new MyIntTuple(80000, 5, 0),
        new MyIntTuple(85000, 5, 1),
        new MyIntTuple(120000, 8, 0),
        new MyIntTuple(121000, 7, 1),
        new MyIntTuple(122000, 4, 2),
        new MyIntTuple(200000, 50, 0),
        new MyIntTuple(200000, 50, 1),
    };

    private void Awake()
    {
        Container.Instance = new Container();
        Container.Instance.AddTowers(towers0, 0);
        Container.Instance.AddTowers(towers1, 1);
        Container.Instance.ProjectileContainer = Container.GetContainer(Config.PROJECTILE_CONTAINER_TAG_NAME);
        Container.Instance.RouteContainer = Container.GetContainer(Config.ROUTE_CONTAINER_TAG_NAME);
        Container.Instance.TowerContainer = Container.GetContainer(Config.TOWER_CONTAINER_TAG_NAME);
        Container.Instance.UnitContainer = Container.GetContainer(Config.UNIT_CONTAINER_TAG_NAME);
        Container.Instance.CreateTowerCanvas = Container.GetContainer(Config.CREATE_TOWER_CANVAS_TAG_NAME).GetComponent<Canvas>();

        CurrentTowerDefenceState.Instance = new CurrentTowerDefenceState();

        var missingCollidersObject = GameObject.FindGameObjectWithTag(Config.MISSING_COLIDERS_CONTAINER_TAG_NAME);
        if (missingCollidersObject != null)
            missingCollidersObject.SetActive(false);
    }

    private void Start()
    {
        lastIncreaseMoneyTime = startedAt = Time.time;
        Time.timeScale = 1.0f;

        var routeContainer = Container.Instance.RouteContainer;
        var collider = mainTower.GetComponentInChildren<Collider>();
        Debug.Assert(collider != null);
        for (var i = 0; i < routeContainer.transform.childCount; ++i) {
            var routerObject = routeContainer.transform.GetChild(i);
            var routerController = routerObject.GetComponent<RouterController>();
            var router = routerController.GetEmptyRouter();
            router.points = new List<Vector3>();
            for (int j = 0; j < routerObject.transform.childCount; ++j)
                router.points.Add(routerObject.transform.GetChild(j).transform.position);
            router.points.Add(mainTower.transform.position);
            router.targetCollider = collider;
            routerController.Router = router;
            routerControllers.Add(routerController);
        }
        for (int i = 0; i < routeContainer.transform.childCount; ++i)
            routeContainer.transform.GetChild(i).gameObject.SetActive(false);
    }


    private void Update()
    {
        TryToSendUnit();
        var balanceDelta = 0;
        while (Time.time - lastIncreaseMoneyTime > Config.ADD_MONEY_INTERVAL) {
            ++balanceDelta;
            lastIncreaseMoneyTime += Config.ADD_MONEY_INTERVAL;
        }
        CurrentTowerDefenceState.Instance.ChangeBalance(balanceDelta);
    }

    public void TryToSendUnit()
    {
        int now = (int)((Time.time - startedAt) * 1000);
        if (lastSendUnitTime + Config.SEND_UNIT_INTERVAL < now && unitWaveIndex < UNIT_WAVES.Count && UNIT_WAVES[unitWaveIndex].first <= now) {
            SendUnit(UNIT_WAVES[unitWaveIndex].third);
            --UNIT_WAVES[unitWaveIndex].second;
            if (UNIT_WAVES[unitWaveIndex].second == 0)
                ++unitWaveIndex;
            lastSendUnitTime = now - Random.Range(0, Config.SEND_UNIT_INTERVAL / 3);
        }
    }

    private void SendUnit(int respawnIndex)
    {
        var routerController = routerControllers[respawnIndex];
        var router = routerController.Router.CopyInstance();
        var units = routerController.units;
        var unit = Instantiate(units[Random.Range(0, units.Length)], Container.Instance.UnitContainer.transform);
        unit.GetComponent<UnitController>().SetUp(router, mainTower);
        router.SetPosition(unit.transform, router.GetInitialPoint());
        routerController.EnableUnit(unit);
    }

    public void CreateOrUpdateTower()
    {
        var state = CurrentTowerDefenceState.Instance;
        Debug.Assert(state.GetBalance() >= Config.CONSTRUCT_TOWER_COST);
        var controller = state.GetSelectedTower().GetComponent<TowerPositionController>();
        state.CreateNextTower(controller);
    }

    public void DeselectTower()
    {
        CurrentTowerDefenceState.Instance.ResetTower();
    }
}
