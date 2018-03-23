using System.Collections.Generic;
using UnityEngine;

public class CurrentTowerDefenceState
{
    private static CurrentTowerDefenceState instance;
    private static Canvas createTowerCanvas;
    private static Canvas changeTowerCanvas;
    public enum UpgradeTypes { UPDRADE_SPEED, UPGRADE_DAMAGE }

    private CurrentTowerDefenceState() { }

    public static CurrentTowerDefenceState GetInstance()
    {
        if (instance == null)
            instance = new CurrentTowerDefenceState();
        return instance;
    }

    public void SetCanvases(Canvas createTower, Canvas changeTower)
    {
        createTowerCanvas = createTower;
        changeTowerCanvas = changeTower;
    }

    public void DisableCanvases()
    {
        createTowerCanvas.GetComponent<Canvas>().enabled = false;
        changeTowerCanvas.GetComponent<Canvas>().enabled = false;
    }

    public bool GetCanvasEnabled()
    {
        return
            createTowerCanvas.GetComponent<Canvas>().enabled ||
            changeTowerCanvas.GetComponent<Canvas>().enabled;
    }

    private GameObject currentTower;
    private Dictionary<string, Tower> createdTowers = new Dictionary<string, Tower>();

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
        return TowerExists() ? changeTowerCanvas : createTowerCanvas;
    }

    public GameObject CreateTower(GameObject towerPrefab)
    {
        var tower = GetCurrentTower();
        Debug.Assert(tower != null);
        Debug.Assert(!createdTowers.ContainsKey(tower.name));
        var newTower = Object.Instantiate(towerPrefab, tower.transform);
        newTower.transform.localPosition = new Vector3(0, 0, 0);
        createdTowers[tower.name] = newTower.GetComponent<Tower>();
        Debug.Log(tower.name);
        DisableCanvases();
        SetCurrentTower(null);
        return newTower;
    }

    public void UpdgradeCurrentTower(UpgradeTypes upgradeType, int value)
    {
        var towerObject = GetCurrentTower();
        var tower = GetCurrentTower().GetComponent<Tower>();
        if (upgradeType == UpgradeTypes.UPGRADE_DAMAGE) {
            tower.damage += value;
        } else if (upgradeType == UpgradeTypes.UPDRADE_SPEED) {
            tower.speed += value;
        }

    }
}
