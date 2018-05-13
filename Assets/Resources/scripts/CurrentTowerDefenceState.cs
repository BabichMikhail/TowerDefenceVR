using System.Collections.Generic;
using UnityEngine;

public class CurrentTowerDefenceState {
    private GameObject selectedTower;
    private Dictionary<string, TowerController> createdTowers = new Dictionary<string, TowerController>();
    private int balance = Config.START_BALANCE;

    public static CurrentTowerDefenceState Instance { get; set; }

    public bool TowerIsCurrent(GameObject tower)
    {
        return selectedTower != null && selectedTower.name == tower.name;
    }

    public void SetSelectedTower(GameObject tower)
    {
        if (selectedTower != null)
            selectedTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>(Config.TOWER_POSITION_SELECTABLE_MODE_MATERIAL);
        selectedTower = tower;
        if (selectedTower != null)
            selectedTower.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>(Config.TOWER_POSITION_SELECTED_MODE_MATERIAL);
    }

    public GameObject GetSelectedTower()
    {
        return selectedTower;
    }

    public Canvas GetConstructionCanvas()
    {
        return Container.Instance.CreateTowerCanvas;
    }

    public void ResetTower()
    {
        GetConstructionCanvas().enabled = false;
        SetSelectedTower(null);
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
        return Container.Instance.GetTowers(type).Length > idx;
    }

    public bool CanCreateTower()
    {
        return Config.CONSTRUCT_TOWER_COST <= balance;
    }

    public void CreateNextTower(TowerPositionController controller)
    {
        var idx = controller.GetCurrentTowerToCreateIndex();
        Debug.Assert(CanCreateNextTower(idx, controller.type));
        var towerPrefab = Container.Instance.GetTowers(controller.type)[idx];
        Object.Instantiate(towerPrefab, GetSelectedTower().transform);
        controller.IncreaseTowerToCreateIndex();
        if (!CanCreateNextTower(controller.GetCurrentTowerToCreateIndex(), controller.type))
            controller.DisableInitialComponents();
        ResetTower();
        ChangeBalance(-Config.CONSTRUCT_TOWER_COST);
        UpdateTowerPositionMaterials();
    }

    public void UpdateTowerPositionMaterials()
    {
        var material = Resources.Load<Material>(Config.TOWER_POSITION_NOT_SELECTABLE_MODE_MATERIAL);
        if (balance >= Config.CONSTRUCT_TOWER_COST)
            material = selectedTower == null ? Resources.Load<Material>(Config.TOWER_POSITION_SELECTABLE_MODE_MATERIAL) : Resources.Load<Material>(Config.TOWER_POSITION_SELECTED_MODE_MATERIAL);

        GameObject[] objects = GameObject.FindGameObjectsWithTag(Config.TOWER_POSITION_TAP_TAG_NAME);
        for (int i = 0; i < objects.Length; ++i) {
            var renderer = objects[i].GetComponent<MeshRenderer>();
            renderer.material = material;
        }
    }
}
