using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainController : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;
    public GameObject mainTower;
    public GameObject[] towers;

    private void Awake()
    {
        Container.GetInstance().SetTowers(towers);
        CurrentTowerDefenceState.GetInstance().SetWorldScale(gameObject.transform.localScale);
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
    }

    private void Update()
    {
        Music.Update();
    }

    public void SendUnit()
    {
        var unit = Instantiate(units[Random.Range(0, units.Length)], Container.GetInstance().GetUnitContainer().transform);
        var router = routers[Random.Range(0, routers.Count)];
        unit.GetComponent<UnitController>().SetUp(router.CopyInstance(), mainTower);
        router.SetPosition(unit.transform, router.GetInitialPoint());
        unit.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void createOrUpdateTower()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        var controller = state.GetCurrentTower().GetComponent<TowerPositionController>();
        state.CreateNextTower(controller);
    }

    public void deselectTower()
    {
        //CurrentTowerDefenceState.GetInstance().SetCurrentTower(null);
        CurrentTowerDefenceState.GetInstance().ResetTower();
    }
}
