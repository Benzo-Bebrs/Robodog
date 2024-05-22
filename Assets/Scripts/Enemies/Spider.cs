using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab, reverseBulletPrefab, bulletCastPrefab;
    [SerializeField] Transform shootPlace, bulletCastPlace;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Trigger playerTrigger;
    [SerializeField] private float timeToStay = 2, timeToShoot = 1;
    [SerializeField] private float speed = 3;
    [SerializeField] private int startState, maxShootsCount = 1;

    private SpriteRenderer sprite;
    private int state, newState, deltState, shootsCount, bulletCastsCount;
    private System.Random rnd;
    private float stayTimer, shootTimer;
    private Player player;
    private Animator animator;

    void Start()
    {
        rnd = new System.Random();  
        state = startState; 
        newState = startState;
        player = Player.Instance;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (newState != state)
        {
            Move();
        }
        else if (playerTrigger.isTriggered)
        {
            Stay();
        }
        else if (!playerTrigger.isTriggered)
        {
            animator.SetInteger("Condition", 0);
        }

        if (bulletCastsCount <= shootsCount && shootsCount < maxShootsCount && shootTimer >= (timeToShoot - 0.2f))
        {
            CastBullet();
        }

        if (!playerTrigger.isTriggered)
        {
            stayTimer = 0;
            shootTimer = 0;
        }
    }

    private void CastBullet()
    {
        bulletCastsCount++;
        Instantiate(bulletCastPrefab, bulletCastPlace.position, Quaternion.identity);
    }

    private void Stay()
    {
        stayTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
        if (shootTimer >= timeToShoot && shootsCount < maxShootsCount)
        {
            shootTimer = 0;
            shootsCount++;
            animator.SetInteger("Condition", 2);
            Invoke(nameof(Shoot), 0.15f);
        }
        else
        {
            animator.SetInteger("Condition", 0);
        }

        if (stayTimer >= timeToStay)
        {
            stayTimer = 0;
            shootTimer = 0;
            ChangePosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        animator.SetInteger("Condition", 1);
        stayTimer = 0;
        deltState = (newState - state) / Math.Abs(newState - state);
        if (transform.position == wayPoints[state + deltState].position)
        {
            state += deltState;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoints[state + deltState].position, Time.deltaTime * speed);
        }
        ChangeSpriteRotate();
    }

    private void ChangeSpriteRotate()
    {
        if (state == 0)
        {
            deltState = 1;
        }
        else if (state == wayPoints.Length - 1)
        {
            deltState = -1;
        }
        Vector3 toNextPos = wayPoints[state + deltState].position - wayPoints[state].position;


        if (toNextPos.y > -0.1f && toNextPos.y < 0.1f) // ползем вправо/влево
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            sprite.flipX = toNextPos.x < 0;
            sprite.flipY = Physics2D.Raycast(transform.position, new Vector2(0, 1), 1, LayerMask.GetMask("Wall")) ||
                Physics2D.Raycast(transform.position, new Vector2(0, 1), 1, LayerMask.GetMask("Ground"));
            shootPlace.localPosition = new Vector3(-0.24f, 0.4f * (sprite.flipY ? -1 : 1), 0);
        }
        else if (toNextPos.x > -0.1f && toNextPos.x < 0.2f) // ползем вверх/вниз 
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            sprite.flipX = toNextPos.y < 0;
            sprite.flipY = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1, LayerMask.GetMask("Wall"));
            shootPlace.localPosition = new Vector3(0.5f * (sprite.flipY ? 1 : -1), 0, 0);
        }
    }

    private void Shoot()
    {
        shootPlace.rotation = new Quaternion(0, 0, 0, 0);
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - shootPlace.position, shootPlace.TransformDirection(Vector3.up));
        shootPlace.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        Instantiate(shootPlace.position.x >= player.transform.position.x ? reverseBulletPrefab : bulletPrefab, shootPlace.position, shootPlace.rotation);
    }

    private void ChangePosition()
    {
        newState = rnd.Next(0, wayPoints.Length);
        if (newState == state)
        {
            shootTimer = 0;
        }
        shootsCount = 0;
        bulletCastsCount = 0;
    }
}
