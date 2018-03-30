using System.Collections.Generic;
using UnityEngine;

public class CurrentTowerDefenceState {
    private static CurrentTowerDefenceState instance;
    private Canvas createTowerCanvas;
    private Canvas updateTowerCanvas;
    private Vector3 worldScale = new Vector3(1.0f, 1.0f, 1.0f);
    public enum UpdateTypes { UPDATE_SPEED, UPDATE_DAMAGE }

    private int balance = 200;

    private CurrentTowerDefenceState()
    {
        createTowerCanvas = Container.GetInstance().GetCreateTowerCanvas();
        updateTowerCanvas = Container.GetInstance().GetUpdateTowerCanvas();
    }

    public static CurrentTowerDefenceState GetInstance()
    {
        if (instance == null)
            instance = new CurrentTowerDefenceState();
        return instance;
    }

    public void SetWorldScale(Vector3 scale) { worldScale = scale; }
    public Vector3 GetWorldScale() { return worldScale; }

    public bool GetCanvasEnabled()
    {
        return
            createTowerCanvas.enabled ||
            updateTowerCanvas.enabled;
    }

    private GameObject currentTower;
    private Dictionary<string, TowerController> createdTowers = new Dictionary<string, TowerController>();

    public bool TowerIsCurrent(GameObject tower)
    {
        return currentTower != null && currentTower.name == tower.name;
    }

    public bool TowerExists()
    {
        return currentTower != null && createdTowers.ContainsKey(currentTower.name);
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
        return TowerExists() ? updateTowerCanvas : createTowerCanvas;
    }

    public void ResetTower()
    {
        createTowerCanvas.enabled = false;
        updateTowerCanvas.enabled = false;
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

    public void UpdateCurrentTower(UpdateTypes updateType, int value)
    {
        var tower = GetCurrentTower().GetComponent<TowerController>();
        if (updateType == UpdateTypes.UPDATE_DAMAGE) {
            tower.damage += value;
        } else if (updateType == UpdateTypes.UPDATE_SPEED) {
            tower.fireInterval -= value;
        }
        ResetTower();
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

    public bool CanCreateNextTower(int idx)
    {
        return Container.GetInstance().GetTowers().Length > idx;
    }

    public bool CanCreateTower()
    {
        return createTowerCost <= balance;
    }

    public const int createTowerCost = 100;

    public void CreateNextTower(TowerPositionController controller)
    {
        var idx = controller.GetCurrentTowerToCreateIndex();
        Debug.Assert(CanCreateNextTower(idx));
        var towerPrefab = Container.GetInstance().GetTowers()[idx];
        var tower = GetCurrentTower();
        var newTower = Object.Instantiate(towerPrefab, tower.transform);
        controller.IncreaseTowerToCreateIndex();
        if (!CanCreateNextTower(controller.GetCurrentTowerToCreateIndex()))
            controller.DisableInitialComponents();
        ResetTower();
        ChangeBalance(-createTowerCost);
        UpdateTowerPositionMaterials();
    }

    public void UpdateTowerPositionMaterials()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("TowerPositionChild");
        var material = Resources.Load<Material>("materials/DarkRedMaterial");
        if (balance >= createTowerCost) {
            material = currentTower == null ? Resources.Load<Material>("materials/Location/Teapot Tower Material") : Resources.Load<Material>("materials/Location/Gear");
        }
        for (int i = 0; i < objects.Length; ++i) {
            var renderer = objects[i].GetComponent<MeshRenderer>();
            renderer.material = material;
        }
        Debug.Log("UPDATE MATERIALS");
    }
}
