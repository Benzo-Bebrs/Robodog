using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private float speed; //скорость перемещения 
    private Player player;
    private Trigger followTrigger1, followTrigger2; // триггеры для отслевживания игрока
    private Rigidbody2D rb;

    private void Start()
    {
        player = Player.Instance;
        followTrigger1 = transform.GetChild(0).GetComponent<Trigger>();
        followTrigger2 = transform.GetChild(1).GetComponent<Trigger>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (followTrigger1.isTriggered || followTrigger2.isTriggered)
        {
            transform.rotation = Quaternion.Euler(0,
                player.transform.position.x >= transform.position.x ? 0 : 180,
                0);
            Vector2 delt = player.transform.position - transform.position;
            rb.velocity = delt.normalized * speed;
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
