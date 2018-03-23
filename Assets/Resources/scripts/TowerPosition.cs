using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPosition : MonoBehaviour {

    private void OnMouseUp()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        var enabled = state.GetCanvasEnabled();
        if (enabled && !state.TowerIsCurrent(gameObject))
            enabled = false;

        state.ResetTower();
        if (!enabled) {
            state.SetCurrentTower(gameObject);
            Canvas canvas = null;
            canvas = state.GetCurrentCanvas();
            canvas.transform.parent = transform;
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            canvas.enabled = true;
        }

        Debug.Log("Click-Click");
    }
}
