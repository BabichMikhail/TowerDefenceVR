using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTowersState
{
    private static CurrentTowersState instance;

    private CurrentTowersState() {}

    public static CurrentTowersState GetInstance()
    {
        if (instance == null)
            instance = new CurrentTowersState();
        return instance;
    }

    private GameObject currentTower;
    private Dictionary<string, bool> createdTowers = new Dictionary<string, bool>();

    public bool TowerIsCurrent(GameObject tower)
    {
        return currentTower != null && currentTower.name == tower.name;
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

    public GameObject CreateTower(GameObject towerPrefab)
    {
        var tower = GetCurrentTower();
        Debug.Assert(tower != null);
        Debug.Assert(!createdTowers.ContainsKey(tower.name));
        var newTower = Object.Instantiate(towerPrefab, tower.transform);
        newTower.transform.localPosition = new Vector3(0, 0, 0);
        createdTowers[tower.name] = true;
        Debug.Log(tower.name);
        return newTower;
    }
}

public class Tower : MonoBehaviour {
    public Canvas createTowerCanvas;
    public Canvas updateTowerCanvas;

    private void OnMouseUp()
    {
        var state = CurrentTowersState.GetInstance();
        var canvas = createTowerCanvas.GetComponent<Canvas>();
        var enabled = canvas.enabled;
        var newEnabled = true;
        if (state.TowerIsCurrent(gameObject))
            newEnabled = !enabled;

        canvas.enabled = false;
        state.SetCurrentTower(null);
        if (newEnabled) {
            createTowerCanvas.transform.parent = transform;
            createTowerCanvas.transform.localPosition = new Vector3(0, 0, 0);
            createTowerCanvas.enabled = true;
            canvas.enabled = true;
            state.SetCurrentTower(gameObject);
        }
        
        Debug.Log("Click-Click");
    }
}
