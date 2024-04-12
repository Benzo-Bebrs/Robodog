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
        player = GetComponentInParent<Player>(); // �������� ��������� Player �� ������������� �������
    }

    private void Update()
    {
        // ���������, �������� �� �������� �� ����������� � ��������� �� �� �����
        isMoving = Mathf.Abs(player.GetHorizontalMovement()) > 0.1f && player.isGrounded;

        if (isMoving && !audioSource.isPlaying)
        {
            // ���� �������� ����� �������� � ���� �� �������������, ������������� ��������� ���� ����
            AudioClip footstepClip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.clip = footstepClip;
            audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        if ((!isMoving || !player.isGrounded) && audioSource.isPlaying)
        {
            // ���� �������� ����������� ��� �� ��������� �� �����, � ���� ��� ��� �������������, ������������� ����
            audioSource.Stop();
        }
    }
}