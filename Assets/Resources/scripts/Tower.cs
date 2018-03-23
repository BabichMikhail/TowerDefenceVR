using UnityEngine;

public class Tower : MonoBehaviour {
    public Canvas createTowerCanvas;
    public Canvas changeTowerCanvas; // TODO remove canvases from here

    private void OnMouseUp()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        state.SetCanvases(createTowerCanvas, changeTowerCanvas); // TODO call once
        var enabled = state.GetCanvasEnabled();
        state.DisableCanvases();
        if (enabled && !state.TowerIsCurrent(gameObject))
            enabled = false;

        state.SetCurrentTower(null);
        if (!enabled) {
            state.SetCurrentTower(gameObject);
            Canvas canvas = null;
            if (state.TowerExists()) {
                canvas = changeTowerCanvas;
                // TODO generate messages
            } else {
                canvas = createTowerCanvas;
            }
            canvas.transform.parent = transform;
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            canvas.enabled = true;
        }
        
        Debug.Log("Click-Click");
    }
}
