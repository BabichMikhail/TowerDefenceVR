﻿using UnityEngine;

public class Unit : MonoBehaviour {
    public BaseRouter router;
    public int health = 5000;
    private float speed = (float)0.20;

    public void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
            CurrentTowerDefenceState.GetInstance().ChangeBalance(100);
        }
            
        if (router.InPlace())
            return; // TODO stop and shoot to main tower;
        var movement = Time.deltaTime * router.GetMovement(transform);
        transform.position = transform.position + (new Vector3(movement.x, 0, movement.y)) * speed;
    }
}
