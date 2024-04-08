using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damageAmount = 1f; //  оличество урона, которое наноситс€ игроку
    public float rechargeTime = 1f; // ¬рем€ перезар€дки перед следующим ударом

    private bool canDamage = true; // ‘лаг, показывающий, можно ли наносить урон в данный момент

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
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