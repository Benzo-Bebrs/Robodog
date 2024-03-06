using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Rigidbody2D rb;
    private float moveSpeed = 10;
    private float jumpSpeed = 5;
    private float moveDirection;

    [Header("Grounded")]
    [SerializeField]private Transform groundCheckPoint;
    [SerializeField] private Vector2 checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private float wallSlideSpeed = 0.3f;
    [SerializeField] private float wallDistance = 0.52f;
    private float jumpTime;
    public bool isWallSliding;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;
    private int extraJumps;
    [SerializeField] private int extraJumpsValue;
    

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
    }

    private void Update()
    {
        if (isGrounded)
            extraJumps = extraJumpsValue;

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isWallSliding) && extraJumps == 0)
        {
            rb.velocity = Vector2.up * jumpSpeed;
        }

        wallCheckHitRight = Physics2D.Raycast(groundCheckPoint.position, new Vector2(wallDistance, 0), wallDistance, whatIsGround);
        wallCheckHitLeft = Physics2D.Raycast(groundCheckPoint.position, new Vector2(-wallDistance, 0), wallDistance, whatIsGround);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, checkRadius, 0f, whatIsGround);

        moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if ((wallCheckHitRight || wallCheckHitLeft) && !isGrounded && moveDirection != 0)
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        //первый вариант отскоков от стен
        if (isWallSliding)
        {
            if (wallCheckHitRight && Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }
            else if (wallCheckHitLeft && Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }
        }

        //немного другой вариант, надо узнать у всех, какой лучше
        //if (isWallSliding)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        //}
    }
}
