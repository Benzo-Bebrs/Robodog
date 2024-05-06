using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } // singleton

    public event Action<float> OnHealthChanged; // Событие смены здоровья
    public event Action OnDeath; // Событие смерти(((
    private float healthPoint = 2;

    private Sounds sounds;
    private bool isSquatSoundPlayed = false;
    private bool isUpSoundPlayed = false;
    private Rigidbody2D rb;
   
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
    public bool isGrounded;
    private bool isWallOnTop;
    public bool isSquat;


    [Header("Jump")]
    [SerializeField] private float wallDistance = 0.52f;
    [SerializeField] private float superJumpCoefficient = 1.5f;
    private bool isWallSliding;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;
    private float jumpTimer;
    private float timeToJump = 0.3f;

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
    private Animator animator;

    public float GetHorizontalMovement()
    {
        // Replace this with the actual code to get the horizontal movement value
        return Input.GetAxis("Horizontal");
    }

    public float HealthPoint
    {
        get { return healthPoint; }
        set
        {
            healthPoint = value;
            OnHealthChanged?.Invoke(healthPoint); // поднимаем ивент изменения здоровья
            if (healthPoint <= 0)
            {
                OnDeath?.Invoke(); // поднимаем ивент смерти
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sounds = GetComponent<Sounds>();
        animator = GetComponent<Animator>();
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
            if (!isSquat && animator.GetInteger("Condition") == 1)
            {
                animator.SetInteger("Condition", -1);
            }
            rb.angularVelocity = 0;
        }
        
        Squat();

        jumpTimer += Time.deltaTime;
    }


    private void Move()
    {
        if (isGrounded)
        {
            animator.SetInteger("Condition", 1);
            rb.angularVelocity = 0;

            if (!isSquat && Math.Abs(rb.velocity.x) < maxMoveSpeed)
            {
                rb.velocity += Vector2.right * moveDirection / 3.0f;
            }

            Turn();
        }
        else
        {
            animator.SetInteger("Condition", -1);
            if (moveDirection != 0) 
            {
                //полёт(перевороты)
                rb.angularVelocity = moveDirection > 0 ? -rotateSpeed : rotateSpeed;       
            }
            else
            {
                rb.angularVelocity = 0;
            }
        }
    }

    private void Squat()
    {
        if (Input.GetKey(KeyCode.S) && isGrounded)
        {
            if (!isSquatSoundPlayed)
            {
                sounds.PlaySound(1, 0.5f, false, false, 0.8f, 1f);
                isSquatSoundPlayed = true;
            }

            isSquat = true;
            rb.sharedMaterial = squatMaterial;
            col.size = squatColliderSize;
            render.sprite = roboSpriteSit;
            if (animator.GetInteger("Condition") != 4)
            {
                animator.SetInteger("Condition", 2);
            }
        }
        else if (isSquat && !isWallOnTop)
        {
            if (!isUpSoundPlayed)
            {
                sounds.PlaySound(2);
                isUpSoundPlayed = true;
            }

            isSquat = false;
            rb.sharedMaterial = normalMaterial;
            col.size = normalColliderSize;
            render.sprite = roboSprite;
            animator.SetInteger("Condition", 3);
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
        if (isGrounded && jumpTimer > timeToJump)
        {
            animator.SetInteger("Condition", 4);
            Invoke(nameof(GroundJump), 0.2f);
            jumpTimer = 0;
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

    private void GroundJump()
    {
        // прыгаем, если в присяде, то прыжок сильнее в superJumpCoefficient раз
        rb.velocity += (isSquat ? superJumpCoefficient : 1) * jumpForce * Vector2.up;
        sounds.PlaySound(0);
    }

    private void TryToShoot()
    {
        if (timeAfterLastShoot >= rechargeTime && !isSquat)
        {
            bool needBost = transform.rotation.eulerAngles.z > 240 && transform.rotation.eulerAngles.z < 300;
            sounds.PlaySound(3);
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
