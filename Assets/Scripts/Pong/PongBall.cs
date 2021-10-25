using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour
{
    public float maxSpeed;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        rigidBody.velocity = new Vector2(10.0f, 1.0f);
    }

    void FixedUpdate()
    {
        // Clamp the speed
        float xSpeed = Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed);
        float ySpeed = Mathf.Clamp(rigidBody.velocity.y, -maxSpeed, maxSpeed);
        rigidBody.velocity = new Vector2(xSpeed, ySpeed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // TODO play sounds depending on what it hit

        if (col.gameObject.GetComponent<PlayerPaddle>() != null || col.gameObject.GetComponent<AIPaddle>() != null)
        {
            // Speed up on paddle impact & add random y speed
            float xSpeed = rigidBody.velocity.x * 1.01f;
            float ySpeed = rigidBody.velocity.y + Random.Range(-1.0f, 1.0f);
            rigidBody.velocity = new Vector2(xSpeed, ySpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Win/Lose
    }
}
