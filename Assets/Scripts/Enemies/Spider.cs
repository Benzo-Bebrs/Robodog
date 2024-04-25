using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab, reverseBulletPrefab;
    [SerializeField] Transform shootPlace;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Trigger playerTrigger;
    [SerializeField] private float timeToMove = 2, timeToShoot = 1;
    [SerializeField] private float speed = 3;

    private int state, newState;
    private System.Random rnd;
    private float moveTimer, shootTimer;
    private Player player;

    void Start()
    {
        rnd = new System.Random();  
        state = 0; 
        newState = 0;
        player = Player.Instance;
    }

    void Update()
    {
        if (newState != state)
        {
            moveTimer = 0;
            int deltState = (newState - state) / Math.Abs(newState - state);
            if (transform.position == wayPoints[state + deltState].position)
            {
                state += deltState;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, wayPoints[state + deltState].position, Time.deltaTime * speed);
            }
        }
        else if (playerTrigger.isTriggered)
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
                changePosition();
            }
        }

        if (!playerTrigger.isTriggered)
        {
            moveTimer = timeToMove;
        }
    }

    private void Shoot()
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - shootPlace.position, shootPlace.TransformDirection(Vector3.up));
        shootPlace.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        Instantiate(shootPlace.position.x >= player.transform.position.x ? reverseBulletPrefab : bulletPrefab, shootPlace.position, shootPlace.rotation);
        shootPlace.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void changePosition()
    {
        newState = rnd.Next(0, wayPoints.Length);
    }
}
