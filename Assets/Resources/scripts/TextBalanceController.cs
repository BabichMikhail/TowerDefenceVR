using UnityEngine;
using UnityEngine.UI;

class TextBalanceController : MonoBehaviour {
    private void Update() {
        gameObject.GetComponent<Text>().text = CurrentTowerDefenceState.GetInstance().GetBalance().ToString();
    }
}