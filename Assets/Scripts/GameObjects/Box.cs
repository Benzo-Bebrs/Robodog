using UnityEngine;

public class MoveBox : MonoBehaviour
{
    public AudioClip movementSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isMoving;
    private bool isPlaying;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = movementSound;
        audioSource.loop = true;
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        isPlaying = false;
    }

    private void FixedUpdate()
    {
        bool wasMoving = isMoving;
        isMoving = rb.velocity.magnitude > 1;

        if (isMoving && !wasMoving)
        {
            StartMovementSound();
        }
        else if (!isMoving && wasMoving)
        {
            StopMovementSound();
        }
    }

    private void StartMovementSound()
    {
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    private void StopMovementSound()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}