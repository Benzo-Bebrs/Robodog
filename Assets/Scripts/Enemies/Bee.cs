using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private float speed; //скорость перемещения 
    private Player player;
    private Trigger followTrigger1, followTrigger2; // триггеры для отслевживания игрока

    private void Awake()
    {
        player = Player.Instance;
        followTrigger1 = transform.GetChild(0).GetComponent<Trigger>();
        followTrigger2 = transform.GetChild(1).GetComponent<Trigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.HealthPoint -= 1;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (followTrigger1.isTriggered || followTrigger2.isTriggered)
        {
            transform.rotation = Quaternion.Euler(0, 
                player.transform.position.x >= transform.position.x ? 0 : 180, 
                0);

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}
