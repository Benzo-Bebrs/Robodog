using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && player.HealthPoint == 1)
            {
                player.HealthPoint += 1;
                Destroy(gameObject);
            }
        }
    }
}