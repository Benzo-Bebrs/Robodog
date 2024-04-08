using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damageAmount = 1f; // ���������� �����, ������� ��������� ������
    public float rechargeTime = 1f; // ����� ����������� ����� ��������� ������

    private bool canDamage = true; // ����, ������������, ����� �� �������� ���� � ������ ������

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