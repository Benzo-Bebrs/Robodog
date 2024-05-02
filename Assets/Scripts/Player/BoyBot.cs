using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBot : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform throwPlace;
    private RealPlayer player;
    private bool isPlayerInArea;
    private Animator animator;
    private int throwNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = RealPlayer.Instance;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInArea = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isUpBall)
        {
            ball.SetActive(false);
        }

        if (isPlayerInArea && player.isUpBall)
        {
            TakeBall();
        }
    }

    private void TakeBall()
    {
        player.isUpBall = false;
        animator.SetInteger("Condition", 1);
        Invoke(nameof(ThrowBall), 1.14f);
    }

    private void ThrowBall()
    {
        ball.transform.position = throwPlace.position;
        ball.SetActive(true);
        ball.GetComponent<Rigidbody2D>().velocity += new Vector2(6.5f + throwNum * 2, 6.5f + throwNum * 2);
        throwNum++;
    }
}
