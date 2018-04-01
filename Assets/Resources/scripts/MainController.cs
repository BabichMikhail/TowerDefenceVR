using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyIntTuple {
    public int first;
    public int second;
    public int third;

    public MyIntTuple(int first, int second, int third) { this.first = first; this.second = second; this.third = third; }
}

public class MainController : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;
    public GameObject mainTower;
    public GameObject[] towers0;
    public GameObject[] towers1;

    private void Awake()
    {
        Container.GetInstance().AddTowers(towers0);
        Container.GetInstance().AddTowers(towers1);
        CurrentTowerDefenceState.GetInstance().SetWorldScale(gameObject.transform.localScale);
        GameObject.FindGameObjectWithTag("MissingColliders").SetActive(false);
    }

    private void Start()
    {
        var routeContainer = Container.GetInstance().GetRouteContainer();
        var collider = mainTower.GetComponentInChildren<Collider>();
        Debug.Assert(collider != null);
        for (var i = 0; i < routeContainer.transform.childCount; ++i) {
            var routerObject = routeContainer.transform.GetChild(i);
            var router = new NavMeshAgentRouter();
            router.points = new List<Vector3>();
            for (int j = 0; j < routerObject.transform.childCount; ++j)
                router.points.Add(routerObject.transform.GetChild(j).transform.position);
            router.points.Add(mainTower.transform.position);
            router.targetCollider = collider;
            routers.Add(router);
        }
        for (int i = 0; i < routeContainer.transform.childCount; ++i)
            routeContainer.transform.GetChild(i).gameObject.SetActive(false);
    }

    private void Update()
    {
        Music.Update();
        TryToSendUnit();
    }

    private int sendUnitsIndex = 0;
    private int lastSendUnitTime = -1000;
    private const int SEND_UNIT_INTERVAL = 200;

    // time in milliseconds, unit count, respawn index
    private List<MyIntTuple> sendUnits = new List<MyIntTuple>() {
        new MyIntTuple(3000, 5, 0),
        new MyIntTuple(3000, 1, 1),
        new MyIntTuple(40000, 2, 0),
        new MyIntTuple(43000, 3, 1),
        new MyIntTuple(80000, 5, 0),
        new MyIntTuple(85000, 5, 1),
        new MyIntTuple(120000, 8, 0),
        new MyIntTuple(121000, 7, 1),
        new MyIntTuple(200000, 50, 0),
        new MyIntTuple(200000, 50, 1),
    };

    public void TryToSendUnit()
    {
        int now = (int)(Time.time * 1000);
        if (lastSendUnitTime + SEND_UNIT_INTERVAL < now && sendUnitsIndex < sendUnits.Count && sendUnits[sendUnitsIndex].first <= now) {
            SendUnit(sendUnits[sendUnitsIndex].third);
            --sendUnits[sendUnitsIndex].second;
            if (sendUnits[sendUnitsIndex].second == 0)
                ++sendUnitsIndex;
            lastSendUnitTime = now - Random.Range(0, SEND_UNIT_INTERVAL / 3);
        }
    }

    private void SendUnit(int respawnIndex)
    {
        var unit = Instantiate(units[Random.Range(0, units.Length)], Container.GetInstance().GetUnitContainer().transform);
        var router = routers[respawnIndex];
        unit.GetComponent<UnitController>().SetUp(router.CopyInstance(), mainTower);
        router.SetPosition(unit.transform, router.GetInitialPoint());
        unit.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void createOrUpdateTower()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        Debug.Assert(state.GetBalance() >= CurrentTowerDefenceState.createTowerCost);
        var controller = state.GetCurrentTower().GetComponent<TowerPositionController>();
        state.CreateNextTower(controller);
    }

    public void deselectTower()
    {
        CurrentTowerDefenceState.GetInstance().ResetTower();
    }
}
