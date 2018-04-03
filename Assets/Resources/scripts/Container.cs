using System.Collections.Generic;
using UnityEngine;

public class Container {
    private static GameObject unitContainer;
    private static GameObject projectileContainer;
    private static GameObject routeContainer;
    private static GameObject towerContainer;
    private static Canvas createTowerCanvas;
    private static Canvas updateTowerCanvas;
    private static List<GameObject[]> towers = new List<GameObject[]>();

    private static Container instance;

    private Container() {}

    public static void Reset() { instance = null; }

    public static Container GetInstance()
    {
        if (instance == null)
            instance = new Container();
        return instance;
    }

    public void AddTowers(GameObject[] towers) { Container.towers.Add(towers);  }
    public GameObject[] GetTowers(int type) { return towers[type]; }
    public GameObject GetUnitContainer()
    {
        if (unitContainer == null)
            unitContainer = GameObject.FindGameObjectWithTag("UnitContainer");
        return unitContainer;
    }
    public GameObject GetProjectileContainer()
    {
        if (projectileContainer == null)
            projectileContainer = GameObject.FindGameObjectWithTag("ProjectileContainer");
        return projectileContainer;
    }
    public GameObject GetRouteContainer()
    {
        if (routeContainer == null)
            routeContainer = GameObject.FindGameObjectWithTag("RouteContainer");
        return routeContainer;
    }
    public GameObject GetTowerContainer() {
        if (towerContainer == null)
            towerContainer = GameObject.FindGameObjectWithTag("TowerContainer");
        return towerContainer;
    }
    public Canvas GetCreateTowerCanvas()
    {
        if (createTowerCanvas == null)
            createTowerCanvas = GameObject.FindGameObjectWithTag("CreateTowerCanvas").GetComponent<Canvas>();
        return createTowerCanvas;
    }
    public Canvas GetUpdateTowerCanvas()
    {
        if (updateTowerCanvas == null)
            updateTowerCanvas = GameObject.FindGameObjectWithTag("UpdateTowerCanvas").GetComponent<Canvas>();
        return updateTowerCanvas;
    }
}
