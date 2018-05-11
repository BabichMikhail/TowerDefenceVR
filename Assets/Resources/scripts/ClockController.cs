using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour {
    private float initializedAt;

    private void Awake()
    {
        initializedAt = Time.time;
    }

    void Update () {
        var startedAt = (int)(Time.time - initializedAt);
        float minutes = startedAt / 60;
        float seconds = startedAt - minutes * 60;
        var secondsString = seconds.ToString();
        if (secondsString.Length == 1)
            secondsString = "0" + secondsString;
        gameObject.GetComponentInChildren<Text>().text = minutes.ToString() + ":" + secondsString;
    }
}
