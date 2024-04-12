using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Player player;
    private bool isMoving;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponentInParent<Player>(); // ѕолучаем компонент Player из родительского объекта
    }

    private void Update()
    {
        // ѕровер€ем, движетс€ ли персонаж по горизонтали и находитс€ ли на земле
        isMoving = Mathf.Abs(player.GetHorizontalMovement()) > 0.1f && player.isGrounded;

        if (isMoving && !audioSource.isPlaying)
        {
            // ≈сли персонаж начал движение и звук не проигрываетс€, воспроизводим случайный звук шага
            AudioClip footstepClip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.clip = footstepClip;
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        if ((!isMoving || !player.isGrounded) && audioSource.isPlaying)
        {
            // ≈сли персонаж остановилс€ или не находитс€ на земле, а звук все еще проигрываетс€, останавливаем звук
            audioSource.Stop();
        }
    }
}