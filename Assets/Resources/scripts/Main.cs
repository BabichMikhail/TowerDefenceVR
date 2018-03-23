using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;
    public GameObject archedTower;
    public GameObject siegeTower;
    public Canvas createTowerCanvas;
    public Canvas changeTowerCanvas;

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
        var unit = Instantiate(units[Random.Range(0, units.Length)], gameObject.transform);
        var router = routers[Random.Range(0, routers.Count)];
        unit.GetComponent<Move>().router = router.CopyInstance();
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
        CurrentTowerDefenceState.GetInstance().CreateTower(archedTower);
    }

    public void createSiegeTower()
    {
        Debug.Log("Siege tower");
        CurrentTowerDefenceState.GetInstance().CreateTower(siegeTower);
    }

    public void updateTowerSpeed()
    {
        Debug.Log("Tower speed");
    }

    public void updateTowerDamage()
    {
        Debug.Log("Tower damage");
    }
}
