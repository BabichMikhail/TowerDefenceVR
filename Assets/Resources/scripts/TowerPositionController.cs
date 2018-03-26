using UnityEngine;

public class TowerPositionController : MonoBehaviour {

    private void OnMouseUp()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        var enabled = state.GetCanvasEnabled();
        if (enabled && !state.TowerIsCurrent(gameObject))
            enabled = false;

        state.ResetTower();
        if (!enabled) {
            state.SetCurrentTower(gameObject);
            Canvas canvas = state.GetCurrentCanvas();
            canvas.transform.SetParent(transform);
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            canvas.enabled = true;
        }
    }
}
