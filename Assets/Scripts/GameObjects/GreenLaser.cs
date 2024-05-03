using UnityEngine;

public class Laser : MonoBehaviour
{
    public float activationTime = 2f; // Время активации лазера
    public float deactivationTime = 1f; // Время деактивации лазера
    public float damageAmount = 1f; // Количество урона, наносимого игроку

    private Collider2D collider;
    private bool isActive = false;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        DeactivateLaser();
        InvokeRepeating("ToggleLaser", activationTime, activationTime + deactivationTime);
    }

    private void ToggleLaser()
    {
        if (isActive)
        {
            DeactivateLaser();
        }
        else
        {
            ActivateLaser();
        }
    }

    private void ActivateLaser()
    {
        isActive = true;
        gameObject.SetActive(true);
        collider.enabled = true;
    }

    private void DeactivateLaser()
    {
        isActive = false;
        gameObject.SetActive(false);
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.HealthPoint -= damageAmount;
                Debug.Log("Player has been hit by the laser! Took " + damageAmount + " damage.");
            }
        }
    }
}