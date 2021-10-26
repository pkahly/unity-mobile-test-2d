using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PongBall : MonoBehaviour
{
    public float maxSpeed;
    public Text playerScoreText;
    public Text enemyScoreText;

    public AudioSource paddleHit;
    public AudioSource wallHit;
    public AudioSource win;
    public AudioSource lose;

    private Rigidbody2D rigidBody;
    private int playerScore = 0;
    private int enemyScore = 0;
    private Vector2 initialPosition;
    private bool isGameOver = true;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        initialPosition = rigidBody.position;
        StartCoroutine(ResetBall());
    }

    IEnumerator ResetBall()
    {
        // Reset the velocity to zero
        rigidBody.velocity = Vector2.zero;

        // Go to the initial position
        rigidBody.position = initialPosition;

        // Wait
        yield return new WaitForSeconds(2);

        // Set the x velocity to half of the maximum but in a random direction
        float xSpeed;
        if (Random.Range(-1f, 1f) >= 0)
        {
            xSpeed = maxSpeed / 2;
        }
        else
        {
            xSpeed = -maxSpeed / 2;
        }

        // Assign initial velocity and start the game
        rigidBody.velocity = new Vector2(xSpeed, Random.Range(-3f, 3f));
        isGameOver = false;
    }

    void FixedUpdate()
    {
        // Clamp the speed
        float xSpeed = Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed);
        float ySpeed = Mathf.Clamp(rigidBody.velocity.y, -maxSpeed, maxSpeed);
        rigidBody.velocity = new Vector2(xSpeed, ySpeed);

        // Set minimum x velocity while the game is in progress
        if (!isGameOver && Mathf.Abs(rigidBody.velocity.x) < 5f)
        {
            if (rigidBody.velocity.x >= 0)
            {
                rigidBody.velocity = new Vector2(5f, rigidBody.velocity.y);
            }
            else
            {
                rigidBody.velocity = new Vector2(-5f, rigidBody.velocity.y);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // TODO play sounds depending on what it hit

        if (col.gameObject.GetComponent<PlayerPaddle>() != null || col.gameObject.GetComponent<AIPaddle>() != null)
        {
            // Speed up on paddle impact & add random y speed
            float xSpeed = rigidBody.velocity.x * 1.05f;
            float ySpeed = rigidBody.velocity.y + Random.Range(-1.0f, 1.0f);
            rigidBody.velocity = new Vector2(xSpeed, ySpeed);

            // Play paddle sound
            paddleHit.Play();
        }
        else
        {
            wallHit.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Prevent double counts
        if (isGameOver)
        {
            return;
        }

        // Win/Lose
        if (other.gameObject.GetComponent<PlayerEndZone>())
        {
            // Play lose sound
            lose.Play();

            isGameOver = true;
            enemyScore++;
            enemyScoreText.text = "" + enemyScore;
            StartCoroutine(ResetBall());
        }
        else if (other.gameObject.GetComponent<EnemyEndZone>())
        {
            // Play win sound
            win.Play();

            isGameOver = true;
            playerScore++;
            playerScoreText.text = "" + playerScore;
            StartCoroutine(ResetBall());
        }
    }
}
