using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Rigidbody2D rb;
    private float maxMoveSpeed = 4;
    private float moveAcceleration = 30;
    private float jumpForce = 5;
    private float reboundForce = 6.5f;
    private float moveDirection;
    private Renderer sprite;

    [Header("Grounded")]
    [SerializeField]private Transform groundCheckPoint, centerCheckPoint;
    [SerializeField] private Vector2 checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float wallDistance = 0.52f;
    public bool isWallSliding;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPlace;
    [SerializeField] private float recoilForce;
    [SerializeField] private float rechargeTime;
    private float timeAfterLastShoot;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void Update()
    {
        wallCheckHitRight = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, whatIsGround);
        wallCheckHitLeft = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryToShoot();
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, checkRadius, 0f, whatIsGround);

        moveDirection = Input.GetAxis("Horizontal");
        if (isGrounded && moveDirection != 0)
        {
            if ((moveDirection > 0 && rb.velocity.x < maxMoveSpeed) || 
                (moveDirection < 0 && rb.velocity.x > -maxMoveSpeed))
            {
                rb.AddForce(Vector2.right * (moveDirection * moveAcceleration), ForceMode2D.Force);
            }
        }

        isWallSliding = (wallCheckHitRight || wallCheckHitLeft) && !isGrounded;
        timeAfterLastShoot += Time.fixedDeltaTime;
    }

    private void TryToJump()
    {
        if (isGrounded)
        {
            rb.velocity += Vector2.up * jumpForce;
        }
        else if (isWallSliding)
        {
            if (wallCheckHitRight)
            {
                rb.velocity += new Vector2(-.75f, 1) * reboundForce;
            }
            else if (wallCheckHitLeft)
            {
                rb.velocity += new Vector2(.75f, 1) * reboundForce;
            }
        }
    }

    private void TryToShoot()
    {
        if (timeAfterLastShoot >= rechargeTime)
        {
            Instantiate(bulletPrefab, shootPlace.position, shootPlace.rotation);
            rb.AddForce(Vector2.left * recoilForce, ForceMode2D.Impulse);
            timeAfterLastShoot = 0;
        }
    }
}
