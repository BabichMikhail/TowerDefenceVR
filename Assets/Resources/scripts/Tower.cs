using UnityEngine;

public class Tower : MonoBehaviour {
    public int damage = 1000;
    public int speed = 1000;

    private void OnMouseUp()
    {
        var state = CurrentTowerDefenceState.GetInstance();
        var enabled = state.GetCanvasEnabled();
        state.DisableCanvases();
        if (enabled && !state.TowerIsCurrent(gameObject))
            enabled = false;

        state.SetCurrentTower(null);
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
