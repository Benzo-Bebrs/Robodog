using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float wallDistance = 0.52f;

    private int state, newState;
    private System.Random rnd;
    private RaycastHit2D wallCheckHitRight, wallCheckHitLeft;
    private float moveTimer, timeToMove;


    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
        timeToMove = 1.5f;
        state = 0; 
        newState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (newState != state && moveTimer < timeToMove)
        {
            wallCheckHitRight = Physics2D.Raycast(transform.position, 
                new Vector2(wallDistance, 0), wallDistance);
            wallCheckHitLeft = Physics2D.Raycast(transform.position, 
                new Vector2(-wallDistance, 0), wallDistance);
            moveTimer += Time.deltaTime;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            changePosition();
        }
    }

    private void changePosition()
    {
        newState = rnd.Next(-2, 3);
        Move();
    }

    private void Jump()
    {

    }

    private void Move()
    {
        if (newState == 1) // идем по часовой стрелке
        {

        }
        else if (newState == -1) // идем против часовой стрелки
        {

        }
    }
}
