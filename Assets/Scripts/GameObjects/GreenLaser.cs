using UnityEngine;

public class Laser : MonoBehaviour
{
    public float activationTime = 2f; // Время активации лазера
    public float deactivationTime = 1f; // Время деактивации лазера

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    private bool isActive = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteRenderer.enabled = true;
        collider.enabled = true;
    }

    private void DeactivateLaser()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            Debug.Log("Player has been hit by the laser!");
        }
    }
}