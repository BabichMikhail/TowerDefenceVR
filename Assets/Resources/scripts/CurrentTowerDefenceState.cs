using System.Collections.Generic;
using UnityEngine;

public class CurrentTowerDefenceState {
    private static CurrentTowerDefenceState instance;
    private Canvas createTowerCanvas;
    private Canvas updateTowerCanvas;
    private Vector3 worldScale = new Vector3(1.0f, 1.0f, 1.0f);
    public enum UpdateTypes { UPDATE_SPEED, UPDATE_DAMAGE }

    public int balance = 100;

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
            currentTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("materials/YelloMaterial");
        currentTower = tower;
        if (currentTower != null)
            currentTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("materials/BlueMaterial");
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

    public void ChangeBalance(int delta)
    {
        balance += delta;
    }

    public bool CanCreateNextTower(int idx)
    {
        return Container.GetInstance().GetTowers().Length > idx;
    }

    public void CreateNextTower(TowerPositionController controller)
    {
        Debug.Log("Hello world");
        //createdTowers[tower.name] = newTower.GetComponent<TowerController>();
        var idx = controller.GetCurrentTowerToCreateIndex();
        Debug.Assert(CanCreateNextTower(idx));
        var towerPrefab = Container.GetInstance().GetTowers()[idx];
        //var gameObject = controller.gameObject;
        var tower = GetCurrentTower();
        var newTower = Object.Instantiate(towerPrefab, tower.transform);
        //newTower.transform.localPosition = new Vector3(0, 0, 0);
        controller.IncreaseTowerToCreateIndex();
        if (!CanCreateNextTower(controller.GetCurrentTowerToCreateIndex()))
            controller.DisableInitialComponents();
        ResetTower();
    }
}
