using System.Collections.Generic;
using UnityEngine;

public class Container {
    private Dictionary<int, GameObject[]> towersByType = new Dictionary<int, GameObject[]>();

    public static Container Instance { get; set; }

    public static GameObject GetContainer(string tagName)
    {
        var container = GameObject.FindGameObjectWithTag(tagName);
        Debug.Assert(container != null);
        return container;
    }

    public GameObject UnitContainer { get; set; }
    public GameObject ProjectileContainer { get; set; }
    public GameObject RouteContainer { get; set; }
    public GameObject TowerContainer { get; set; }
    public Canvas CreateTowerCanvas { get; set; }

    public void AddTowers(GameObject[] towers, int type) { towersByType[type] = towers;  }
    public GameObject[] GetTowers(int type) { return towersByType[type]; }
}
