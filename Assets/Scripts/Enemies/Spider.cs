using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab, reverseBulletPrefab;
    [SerializeField] Transform shootPlace;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Trigger playerTrigger;
    [SerializeField] private float timeToMove = 2, timeToShoot = 1;
    [SerializeField] private float speed = 3;

    private SpriteRenderer sprite;
    private int state, newState, deltState;
    private System.Random rnd;
    private float moveTimer, shootTimer;
    private Player player;

    void Start()
    {
        rnd = new System.Random();  
        state = 0; 
        newState = 0;
        player = Player.Instance;
        sprite = GetComponentInChildren<SpriteRenderer>();
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

        if (!playerTrigger.isTriggered)
        {
            moveTimer = timeToMove;
        }
    }

    private void Stay()
    {
        moveTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
        if (shootTimer >= timeToShoot)
        {
            shootTimer = 0;
            Shoot();
        }
        if (moveTimer >= timeToMove)
        {
            moveTimer = 0;
            shootTimer = timeToShoot;
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
        moveTimer = 0;
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
        }
        else if (toNextPos.x > -0.1f && toNextPos.x < 0.2f) // ползем вверх/вниз 
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            sprite.flipX = toNextPos.y < 0;
            sprite.flipY = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1, LayerMask.GetMask("Wall"));
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
    }
}
