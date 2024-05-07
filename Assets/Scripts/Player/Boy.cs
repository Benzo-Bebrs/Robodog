using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour
{
    public static Boy Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;

    private Animator animator;
    private Vector3 lastMoveDirection = Vector3.right;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
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

        animator.SetInteger("Condition", horizontalInput != 0 ? 1 : 0);

        transform.localScale = new Vector3(Mathf.Sign(lastMoveDirection.x) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
