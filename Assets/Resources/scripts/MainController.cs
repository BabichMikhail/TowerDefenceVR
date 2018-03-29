using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainController : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;
    public GameObject mainTower;
    public GameObject archedTower;
    public GameObject archedProjectile;
    public GameObject siegeTower;
    public GameObject siegeProjectile;

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
        CurrentTowerDefenceState.GetInstance().UpdateCurrentTower(CurrentTowerDefenceState.UpdateTypes.UPDATE_SPEED, 1);
    }

    public void increaseTowerDamage()
    {
        Debug.Log("Tower damage");
        CurrentTowerDefenceState.GetInstance().UpdateCurrentTower(CurrentTowerDefenceState.UpdateTypes.UPDATE_DAMAGE, 1);
    }
}
