using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        rigidBody.velocity = new Vector2(2.0f, 10.0f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // TODO play sounds
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
    }
}
