using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Joystick that controls the player
    public Joystick joystick;

    // Camera Boundaries
    float maxX = 8.36f;
    float minX = -8.36f;
    float maxY = 4.5f;
    float minY = -4.5f;

    float speed = 0.2f;


    void Start() {
        
    }

    void Update() {
        float x = transform.position.x + joystick.Horizontal * speed;
        float y = transform.position.y + joystick.Vertical * speed;

        transform.position = new Vector3(Mathf.Clamp(x, minX, maxX), Mathf.Clamp(y, minY, maxY), 0);
    }
}
