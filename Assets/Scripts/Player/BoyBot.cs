using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBot : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform throwPlace;
    [SerializeField] private GameObject objectToDestroy; 
    [SerializeField] private GameObject phraseObject; 
    [SerializeField] private Transform phrasePlace; 
    private RealPlayer player;
    private bool isPlayerInArea;
    private Animator animator;
    private int throwNum = 0;

    void Start()
    {
        player = RealPlayer.Instance;
        animator = GetComponent<Animator>();
        ThrowBall();
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
        if (throwNum == 0)
        {
            Instantiate(phraseObject, phrasePlace.position, Quaternion.identity);
        }
        ball.transform.position = throwPlace.position;
        ball.SetActive(true);
        ball.GetComponent<Rigidbody2D>().velocity += new Vector2(6.5f + throwNum * 2, 6.5f + throwNum * 2);
        throwNum++;

        if (throwNum == 4)
        {
            Destroy(objectToDestroy);
        }
    }
}