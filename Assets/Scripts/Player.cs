using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Rigidbody2D rb;
    private float maxMoveSpeed = 7.5f;
    private float jumpForce = 5;
    private float reboundForce = 6.5f;
    private float moveDirection;
    private float rotateDirection;
    private float rotateSpeed = 180;
    private bool canRotation;

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
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, checkRadius, 0f, whatIsGround);

        isWallSliding = (wallCheckHitRight || wallCheckHitLeft) && !isGrounded;
        timeAfterLastShoot += Time.fixedDeltaTime;
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

        moveDirection = Input.GetAxis("Horizontal");
        rotateDirection = Input.GetAxis("Vertical");
        if (moveDirection != 0 || rotateDirection != 0)
        {
            Move();
        }
    }

    private void Move()
    {
        if (isGrounded)
        {
            if (Math.Abs(rb.velocity.x) < maxMoveSpeed)
            {
                rb.velocity += Vector2.right * moveDirection / 5.0f;
            }

            if (moveDirection < 0 && transform.rotation.eulerAngles.y == 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
            }
            else if (moveDirection >= 0 && transform.rotation.eulerAngles.y == 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            }
        }
        //else if(canRotation)
        //{
        //    if (rotateDirection > 0)
        //    {
        //        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        //    }
        //    else if (rotateDirection < 0)
        //    {
        //        transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        //    }

        //    if ((transform.rotation.eulerAngles.z % 360 > 0 && transform.rotation.eulerAngles.z % 360 < 90) || 
        //        (transform.rotation.eulerAngles.z % 360 > 270))
        //    {
        //        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        //    }
        //}
    }

    private void TryToJump()
    {
        canRotation = true;
        if (isGrounded)
        {
            rb.velocity += Vector2.up * jumpForce;
        }
        else if (isWallSliding)
        {
            if (wallCheckHitRight)
            {
                rb.velocity += new Vector2(-.75f, 1) * reboundForce;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (wallCheckHitLeft)
            {
                rb.velocity += new Vector2(.75f, 1) * reboundForce;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void TryToShoot()
    {
        if (timeAfterLastShoot >= rechargeTime)
        {
            Instantiate(bulletPrefab, shootPlace.position, shootPlace.rotation);
            rb.AddRelativeForce(- new Vector3(transform.rotation.x, transform.rotation.y).normalized * recoilForce, ForceMode2D.Impulse);
            timeAfterLastShoot = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            canRotation = false;
        }
    }
}
