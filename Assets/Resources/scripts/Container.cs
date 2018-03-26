using UnityEngine;

public class Container {
    private static GameObject unitContainer;
    private static GameObject projectileContainer;
    private static GameObject routeContainer;
    private static GameObject towerContainer;
    private static Canvas createTowerCanvas;
    private static Canvas updateTowerCanvas;

    private static Container instance;

    private Container()
    {
        unitContainer = GameObject.FindGameObjectWithTag("UnitContainer");
        projectileContainer = GameObject.FindGameObjectWithTag("ProjectileContainer");
        routeContainer = GameObject.FindGameObjectWithTag("RouteContainer");
        towerContainer = GameObject.FindGameObjectWithTag("TowerContainer");
        createTowerCanvas = GameObject.FindGameObjectWithTag("CreateTowerCanvas").GetComponent<Canvas>();
        updateTowerCanvas = GameObject.FindGameObjectWithTag("UpdateTowerCanvas").GetComponent<Canvas>();
    }

    public static Container GetInstance()
    {
        if (instance == null)
            instance = new Container();
        return instance;
    }

    public GameObject GetUnitContainer() { return unitContainer; }
    public GameObject GetProjectileContainer() { return projectileContainer; }
    public GameObject GetRouteContainer() { return routeContainer; }
    public GameObject GetTowerContainer() { return towerContainer; }
    public Canvas GetCreateTowerCanvas() { return createTowerCanvas; }
    public Canvas GetUpdateTowerCanvas() { return updateTowerCanvas; }
}
