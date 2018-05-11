using System.Collections.Generic;
using UnityEngine;

public class CurrentTowerDefenceState {
    private static CurrentTowerDefenceState instance;
    private GameObject currentTower;
    private Dictionary<string, TowerController> createdTowers = new Dictionary<string, TowerController>();
    private int balance = Config.START_BALANCE;


    public enum UpdateTypes { UPDATE_SPEED, UPDATE_DAMAGE }

    private CurrentTowerDefenceState() {}
    public static void Reset() { instance = null; }

    public static CurrentTowerDefenceState GetInstance()
    {
        if (instance == null)
            instance = new CurrentTowerDefenceState();
        return instance;
    }

    public bool TowerIsCurrent(GameObject tower)
    {
        return currentTower != null && currentTower.name == tower.name;
    }

    public void SetCurrentTower(GameObject tower)
    {
        if (currentTower != null)
            currentTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("materials/Location/Teapot Tower Material");
        currentTower = tower;
        if (currentTower != null)
            currentTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("materials/Location/Gear");
    }

    public GameObject GetCurrentTower()
    {
        return currentTower;
    }

    public Canvas GetCurrentCanvas()
    {
        return Container.GetInstance().GetCreateTowerCanvas();
    }

    public void ResetTower()
    {
        GetCurrentCanvas().enabled = false;
        SetCurrentTower(null);
    }

    public GameObject CreateTower(GameObject towerPrefab, GameObject projectile)
    {
        var tower = GetCurrentTower();
        Debug.Assert(tower != null);
        Debug.Assert(!createdTowers.ContainsKey(tower.name));
        var newTower = Object.Instantiate(towerPrefab, tower.transform);
        newTower.GetComponent<TowerController>().SetProjectile(projectile);
        newTower.transform.localPosition = new Vector3(0, 0, 0);
        createdTowers[tower.name] = newTower.GetComponent<TowerController>();
        ResetTower();
        return newTower;
    }

    public int GetBalance()
    {
        return balance;
    }

    public void ChangeBalance(int delta)
    {
        balance += delta;
        UpdateTowerPositionMaterials();
    }

    public bool CanCreateNextTower(int idx, int type)
    {
        return Container.GetInstance().GetTowers(type).Length > idx;
    }

    public bool CanCreateTower()
    {
        return Config.CONSTRUCT_TOWER_COST <= balance;
    }

    public void CreateNextTower(TowerPositionController controller)
    {
        var idx = controller.GetCurrentTowerToCreateIndex();
        Debug.Assert(CanCreateNextTower(idx, controller.type));
        var towerPrefab = Container.GetInstance().GetTowers(controller.type)[idx];
        Object.Instantiate(towerPrefab, GetCurrentTower().transform);
        controller.IncreaseTowerToCreateIndex();
        if (!CanCreateNextTower(controller.GetCurrentTowerToCreateIndex(), controller.type))
            controller.DisableInitialComponents();
        ResetTower();
        ChangeBalance(-Config.CONSTRUCT_TOWER_COST);
        UpdateTowerPositionMaterials();
    }

    public void UpdateTowerPositionMaterials()
    {
        var material = Resources.Load<Material>("materials/DarkRedMaterial");
        if (balance >= Config.CONSTRUCT_TOWER_COST)
            material = currentTower == null ? Resources.Load<Material>("materials/Location/Teapot Tower Material") : Resources.Load<Material>("materials/Location/Gear");

        GameObject[] objects = GameObject.FindGameObjectsWithTag("TowerPositionChild");
        for (int i = 0; i < objects.Length; ++i) {
            var renderer = objects[i].GetComponent<MeshRenderer>();
            renderer.material = material;
        }
    }
}
