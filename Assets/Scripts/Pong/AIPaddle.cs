using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float min;
    public float max;
    public Transform ball;

    void Update()
    {
        float ballY = ball.position.y;

        transform.position = new Vector2(transform.position.x, Mathf.Clamp(ballY, min, max));
    }
}
