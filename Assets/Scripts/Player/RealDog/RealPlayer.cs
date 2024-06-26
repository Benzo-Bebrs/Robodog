using System;
using System.Collections;
using UnityEngine;

public class RealPlayer : MonoBehaviour
{
    public static RealPlayer Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.5f;

    private Animator animator;
    private Vector3 lastMoveDirection = Vector3.right;
    private bool canUpBall;
    public bool isUpBall;
    private bool canPlayFootstep = true;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            canUpBall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            canUpBall = false;
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
        CamController.cameraShake?.Invoke(1f, 1f, 1f);

        if (horizontalInput > 0)
        {
            lastMoveDirection = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            lastMoveDirection = Vector3.left;
        }

        animator.SetInteger("Condition", horizontalInput != 0 ? (isUpBall ? 2 : 1) : (isUpBall ? -1 : 0));

        if (canUpBall)
        {
            animator.SetInteger("Condition", 3);
            Invoke(nameof(UpBall), 0.4f);
        }

        PlayFootstepSound(horizontalInput);
        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void UpBall()
    {
        isUpBall = true;
    }

    private void PlayFootstepSound(float horizontalInput)
    {
        if (horizontalInput != 0 && canPlayFootstep && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = footstepSound;
            GetComponent<AudioSource>().Play();
            StartCoroutine(DelayNextFootstep());
        }
    }

    private IEnumerator DelayNextFootstep()
    {
        canPlayFootstep = false;
        yield return new WaitForSeconds(footstepDelay);
        canPlayFootstep = true;
    }
}