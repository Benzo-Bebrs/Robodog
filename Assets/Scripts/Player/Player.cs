using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } // singleton

    public AudioClip moveSound;
    public AudioClip sitSound;
    public AudioClip upSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip rollSound;
    public AudioClip rollStopSound;
    public AudioClip shootSound;
    private bool isSquatSoundPlayed = false;
    private bool isUpSoundPlayed = false;

    private Rigidbody2D rb;
    public float healthPoint = 3;
    private float maxMoveSpeed = 7.5f; // максимальная скорость движения по горизонтали
    private float jumpForce = 8f; // сила прыжка 
    private float reboundForce = 7.5f; //сила прыжка от стены
    private float moveDirection; // направление движения (W/D и стрелки)
    private float rotateSpeed = 310; // скорость вращения 
    private CapsuleCollider2D col;
    private Vector2 squatColliderSize = new(1.8f, 0.75f);
    private Vector2 normalColliderSize = new(1.8f, 1.22f);

    [Header("Grounded")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform upCheckPoint;
    [SerializeField] private Vector2 checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D squatMaterial;
    private bool isGrounded;
    private bool isWallOnTop;
    private bool isSquat;


    [Header("Jump")]
    [SerializeField] private float wallDistance = 0.52f;
    [SerializeField] private float superJumpCoefficient = 1.5f;
    private bool isWallSliding;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletReversePrefab;
    [SerializeField] private Transform shootPlace;
    [SerializeField] private float recoilForce;
    [SerializeField] private float rechargeTime;
    private float timeAfterLastShoot;

    [Header("Sprites")]
    [SerializeField] private Sprite roboSprite;
    [SerializeField] private Sprite roboSpriteSit;
    [SerializeField] private SpriteRenderer render;

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        healthPoint -= amount;
        if (healthPoint <= 0)
        {
            Debug.Log("Player dead");
        }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, checkRadius, 0f, whatIsGround);
        isWallOnTop = Physics2D.OverlapBox(upCheckPoint.position, checkRadius / 2.0f, 0f, whatIsGround);

        isWallSliding = (wallCheckHitRight || wallCheckHitLeft) && !isGrounded;
        timeAfterLastShoot += Time.fixedDeltaTime;
        
        rb.freezeRotation = isGrounded;
    }

    private void Update()
    {
        if (isGrounded && transform.rotation.eulerAngles.z != 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            AudioSource.PlayClipAtPoint(landingSound, transform.position);
            //приземление
        }

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

            if (!isSquat && Math.Abs(rb.velocity.x) < maxMoveSpeed)
            {
                rb.velocity += Vector2.right * moveDirection / 3.0f;
            }

            Turn();
        }
        else
        {
            if (moveDirection != 0) 
            {
                rb.angularVelocity = moveDirection > 0 ? rotateSpeed : -rotateSpeed;       
                //полёт(перевороты)
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
            if (!isSquatSoundPlayed)
            {
                AudioSource.PlayClipAtPoint(sitSound, transform.position);
                isSquatSoundPlayed = true;
            }

            isSquat = true;
            rb.sharedMaterial = squatMaterial;
            col.size = squatColliderSize;
            render.sprite = roboSpriteSit;
        }
        else if (isSquat && !isWallOnTop)
        {
            if (!isUpSoundPlayed)
            {
                AudioSource.PlayClipAtPoint(upSound, transform.position);
                isUpSoundPlayed = true;
            }

            isSquat = false;
            rb.sharedMaterial = normalMaterial;
            col.size = normalColliderSize;
            render.sprite = roboSprite;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            isSquatSoundPlayed = false;
            isUpSoundPlayed = false;
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
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else if (isWallSliding)
        {
            // если стена справа, то отпрыгиваем влево, если слева, то вправо
            if (wallCheckHitRight)
            {
                rb.velocity += new Vector2(-.75f, 1) * reboundForce;
                //разворот в противоположную сторону при отталкивании
                //transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (wallCheckHitLeft)
            {
                rb.velocity += new Vector2(.75f, 1) * reboundForce;
                //разворот в противоположную сторону при отталкивании
                //transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void TryToShoot()
    {
        if (timeAfterLastShoot >= rechargeTime && !isSquat)
        {
            bool needBost = transform.rotation.eulerAngles.z > 240 && transform.rotation.eulerAngles.z < 300;
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            Vector3 recoilVector = new Vector3((transform.rotation.eulerAngles.y == 0 ? -1 : 1) * (float)Math.Cos(transform.rotation.eulerAngles.z * Math.PI / 180.0),
                -(float)Math.Sin(transform.rotation.eulerAngles.z * Math.PI / 180.0), 0);
            
            // спавним пулю, надо пофиксить, смотрим влево - летит не туда 
            Instantiate(transform.rotation.y == 0 ? bulletPrefab : bulletReversePrefab, 
                shootPlace.position, transform.rotation);
            
            // добавляем отдачу с силой recoilForce
            rb.AddForce(recoilVector * recoilForce * (isGrounded ? 0.8f : 1) * (needBost ? 1.5f : 1), ForceMode2D.Impulse);
            timeAfterLastShoot = 0;
        }
    }
}
