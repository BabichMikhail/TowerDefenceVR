using UnityEngine;

class Animation {
    public static void TryAnimate(GameObject gameObject, string action)
    {
        var animator = gameObject.GetComponent<Animator>();
        if (animator != null)
            animator.SetBool(action, true);
    }
}