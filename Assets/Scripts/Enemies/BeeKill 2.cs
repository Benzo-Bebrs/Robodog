using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeKill : MonoBehaviour
{
    private Player player;
    private Bee bee;

    private void Start()
    {
        bee = GetComponentInParent<Bee>();
        player = Player.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.HealthPoint -= 1;
            Destroy(bee.gameObject);
        }
        else if (collision.CompareTag("Bullet"))
        {
            Destroy(bee.gameObject);
        }
    }
}
