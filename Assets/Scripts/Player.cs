using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } // singleton

    private Rigidbody2D rb;
    private float maxMoveSpeed = 7.5f; // максимальная скорость движения по горизонтали
    private float jumpForce = 8f; // сила прыжка 
    private float reboundForce = 7.5f; //сила прыжка от стены
    public float moveDirection; // направление движения (W/D и стрелки)
    private float rotateSpeed = 240; // скорость вращения 
    private BoxCollider2D col;
    private Vector2 squatColliderSize = new(1.8f, 0.75f);
    private Vector2 normalColliderSize = new(1.8f, 1.22f);

    [Header("Grounded")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D squatMaterial;
    private bool isGrounded;
    private bool isSquat;

    [Header("Jump")]
    [SerializeField] private float wallDistance = 0.52f;
    [SerializeField] private float superJumpCoefficient = 1.5f;
    private bool isWallSliding;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPlace;
    [SerializeField] private float recoilForce;
    [SerializeField] private float rechargeTime;
    private float timeAfterLastShoot;

    [Header("Sprites")]
    [SerializeField] private Sprite roboSprite;
    [SerializeField] private Sprite roboSpriteSit;
    [SerializeField] private SpriteRenderer render;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
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

        Squat();
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

            if (!isSquat && Math.Abs(rb.velocity.x) < maxMoveSpeed)
            {
                rb.velocity += Vector2.right * moveDirection / 5.0f;
            }

            Turn();
        }
        else
        {
            if (moveDirection != 0)
            {
                rb.angularVelocity = moveDirection > 0 ? rotateSpeed : -rotateSpeed;
            }
            else
            {
                rb.angularVelocity = 0;
            }
        }
    }

    private void Squat()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isSquat = true;
            rb.sharedMaterial = squatMaterial;
            col.size = squatColliderSize;
            render.sprite = roboSpriteSit;
        }
        else if (isSquat) {
            isSquat = false;
            rb.sharedMaterial = normalMaterial;
            col.size = normalColliderSize;
            render.sprite = roboSprite;
        }
    }

    private void Turn()
    {
        if (moveDirection <= 0 && transform.rotation.eulerAngles.y != 180)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveDirection > 0 && transform.rotation.eulerAngles.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void TryToJump()
    {
        if (isGrounded)
        {
            // прыгаем, если в присяде, то прыжок сильнее в superJumpCoefficient раз
            rb.velocity += (isSquat ? superJumpCoefficient : 1) * jumpForce * Vector2.up; 
        }
        else if (isWallSliding)
        {
            // если стена справа, то отпрыгиваем влево, если слева, то вправо
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
            // спавним пулю
            Instantiate(bulletPrefab, shootPlace.position, shootPlace.rotation);
            // добавляем отдачу с силой recoilForce
            rb.AddForce(new Vector3((transform.rotation.eulerAngles.y == 0 ? -1 : 1) * (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI / 180.0),
                -(float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI / 180.0), 0) * recoilForce * (isGrounded ? 0.5f : 1), ForceMode2D.Impulse);
            timeAfterLastShoot = 0;
        }
    }
}
