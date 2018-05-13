using UnityEngine;
using System.Collections.Generic;

public class TowerPositionController : MonoBehaviour {
    public int type = 0;
    public GameObject shootAtCenterPoint;

    private int towerToCreateIndex = 0;
    private bool disabled = false;

    private List<GameObject> initialComponents = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
            if (transform.GetChild(i).tag != "CenterPoint")
                initialComponents.Add(transform.GetChild(i).gameObject);
    }

    private void OnMouseUp()
    {
        if (disabled || !CurrentTowerDefenceState.Instance.CanCreateTower() || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        var state = CurrentTowerDefenceState.Instance;
        var enabled = state.GetConstructionCanvas().enabled;
        if (enabled && !state.TowerIsCurrent(gameObject))
            enabled = false;

        state.ResetTower();
        if (!enabled) {
            state.SetSelectedTower(gameObject);
            Canvas canvas = state.GetConstructionCanvas();
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            canvas.enabled = true;
        }
    }

    public int GetCurrentTowerToCreateIndex()
    {
        return towerToCreateIndex;
    }

    public void IncreaseTowerToCreateIndex()
    {
        ++towerToCreateIndex;
    }

    public void DisableInitialComponents()
    {
        foreach (var obj in initialComponents)
            Destroy(obj);
        disabled = true;
    }

    public Vector3 GetShootAtCenterPoint()
    {
        return shootAtCenterPoint.transform.position;
    }
}
