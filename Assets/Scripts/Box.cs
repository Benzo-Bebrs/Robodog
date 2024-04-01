using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip movementSound; // Звук движения
    private AudioSource audioSource;
    private bool isPlaying;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPlaying = false;
    }

    private void Update()
    {
        // Проверяем, двигается ли объект со скоростью больше 1
        if (GetComponent<Rigidbody2D>().velocity.magnitude > 1)
        {
            if (!isPlaying)
            {
                PlayMovementSound();
            }
        }
        else
        {
            if (isPlaying)
            {
                StopMovementSound();
            }
        }
    }

    private void PlayMovementSound()
    {
        audioSource.clip = movementSound;
        audioSource.loop = true; // Зацикливаем звук
        audioSource.Play();
        isPlaying = true;
    }

    private void StopMovementSound()
    {
        audioSource.Stop();
        isPlaying = false;
    }
}