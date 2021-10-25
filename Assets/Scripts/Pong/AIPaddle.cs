using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public float min;
    public float max;
    public Transform ball;
    public float reactionDist;
    public float movementSpeed;

    void Update()
    {
        float distance = Mathf.Abs(transform.position.x - ball.position.x);
        if (distance < reactionDist)
        {
            float ballY = ball.position.y;
            Vector2 target = new Vector2(transform.position.x, Mathf.Clamp(ballY, min, max));
            transform.position = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
        }
    }
}
