using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } // singleton

    public float cos;
    public float sin;

    private Rigidbody2D rb;
    private float maxMoveSpeed = 7.5f; // максимальная скорость движения по горизонтали
    private float jumpForce = 5f; // сила прыжка 
    private float reboundForce = 7.5f; //сила прыжка от стены
    public float moveDirection; // направление движения (W/D и стрелки)
    private float rotateSpeed = 180; // скорость вращения 

    [Header("Grounded")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    public bool isGrounded;

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
        
        rb.freezeRotation = isGrounded;
    }

    private void Update()
    {
        cos = (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI / 180.0);
        sin = (float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI / 180.0);
        wallCheckHitRight = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, whatIsWall);
        wallCheckHitLeft = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, whatIsWall);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryToShoot();
        }

        moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection != 0)
        {
            Move();
        }
        else
        {
            rb.angularVelocity = 0;
        }
    }

    private void Move()
    {
        if (isGrounded)
        {
            rb.angularVelocity = 0;
            if (transform.rotation.eulerAngles.z != 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
            
            if (Math.Abs(rb.velocity.x) < maxMoveSpeed)
            {
                rb.velocity += Vector2.right * moveDirection / 5.0f;
            }

            if (moveDirection <= 0 && transform.rotation.eulerAngles.y != 180)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveDirection > 0 && transform.rotation.eulerAngles.y != 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (moveDirection > 0)
            {
                rb.angularVelocity = rotateSpeed;
                //transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            }
            else if (moveDirection < 0)
            {
                rb.angularVelocity = -rotateSpeed;
                //transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
            }
            else
            {
                rb.angularVelocity = 0;
            }
        }
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
            cos = (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI / 180.0);
            sin = (float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI / 180.0);
            Instantiate(bulletPrefab, shootPlace.position, shootPlace.rotation);
            //rb.AddRelativeForce(Vector3.left * recoilForce, ForceMode2D.Impulse);
            rb.AddForce(new Vector3((transform.rotation.eulerAngles.y == 0 ? -1 : 1) * cos, - sin, 0) * recoilForce, ForceMode2D.Impulse);
            timeAfterLastShoot = 0;
        }
    }
}
