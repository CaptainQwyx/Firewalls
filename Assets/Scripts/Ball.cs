using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // config params
    [SerializeField] Paddle paddle = null;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[] ballSounds = null;
    [SerializeField] float randomBounceFactor = 0.2f;
    [SerializeField] int speedDelta = 0;
    [SerializeField] int minSpeedDelta = -5;
    [SerializeField] int maxSpeedDelta = 5;
    [SerializeField] float speedFactorIncrement = 0.1f;

    // state
    Vector2 paddleToBallVector;
    bool ballLaunched = false;
    float velocityFactor = 1f;

    // Cached component references
    AudioSource myAudioSource = null;
    Rigidbody2D myRigidBody2D = null;
    GameSession theGameSession = null;


    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle.transform.position;
        ballLaunched = false;
        velocityFactor = (1 + speedFactorIncrement);

        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        theGameSession = FindObjectOfType<GameSession>();

        AdjustScoreFactor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckVelocity();

        if (!ballLaunched)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void CheckVelocity()
    {
        // Adjust the ball's speed up or down on left or right mouse button push
        if (Input.GetMouseButtonDown(0))
        {
            if (speedDelta < maxSpeedDelta)
            {
                speedDelta++;
                AdjustScoreFactor();

                if (ballLaunched)
                {
                    Vector2 v = myRigidBody2D.velocity;
                    myRigidBody2D.velocity = new Vector2(v.x * velocityFactor, v.y * velocityFactor);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (speedDelta > minSpeedDelta)
            {
                speedDelta--;
                AdjustScoreFactor();

                if (ballLaunched)
                {
                    Vector2 v = myRigidBody2D.velocity;
                    myRigidBody2D.velocity = new Vector2(v.x / velocityFactor, v.y / velocityFactor);
                }
            }
        }
    }

    private void AdjustScoreFactor()
    {
        theGameSession.AdjustScoreFactor(1 + (speedDelta * speedFactorIncrement));
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidBody2D.velocity = new Vector2(
                xPush * Mathf.Pow(velocityFactor, speedDelta),
                yPush * Mathf.Pow(velocityFactor, speedDelta)
                );
            ballLaunched = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ballLaunched)
        {
            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);

            // keep the ball from stagnating
            Vector2 velocityTweak = new Vector2(
                Random.Range(-randomBounceFactor, randomBounceFactor), 
                Random.Range(-randomBounceFactor, randomBounceFactor)
                );
            myRigidBody2D.velocity += velocityTweak;
        }
    }
}
