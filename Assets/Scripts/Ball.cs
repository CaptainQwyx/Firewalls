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

    // state
    Vector2 paddleToBallVector;
    bool ballLaunched = false;

    // Cached component references
    AudioSource myAudioSource = null;
    Rigidbody2D myRigidBody2D = null;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle.transform.position;
        ballLaunched = false;

        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ballLaunched)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
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
                Random.Range(0, randomBounceFactor), 
                Random.Range(0, randomBounceFactor)
                );
            myRigidBody2D.velocity += velocityTweak;
        }
    }
}
