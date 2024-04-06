using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public Player player; // ������ �� ������ Player, ��� �������� �������� ������
    public TextMeshProUGUI healthText; // ������ �� ��������� TextMeshProUGUI �� �������

    private void Update()
    {
        // ��������� ��������� ���� �������� � ������� ��������� healthPoint �� ������� Player
        healthText.text = "Health: " + player.healthPoint.ToString();
    }
}