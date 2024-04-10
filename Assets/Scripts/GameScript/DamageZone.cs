using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damageAmount = 1f; // Amount of damage to inflict on the player
    public float rechargeTime = 1f; // Time delay before the next hit can be inflicted

    private bool canDamage = true; // Flag indicating if damage can be inflicted at the moment

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.HealthPoint -= damageAmount;
                canDamage = false;
                Invoke(nameof(Recharge), rechargeTime);
            }
        }
    }

    private void Recharge()
    {
        canDamage = true;
    }
}
