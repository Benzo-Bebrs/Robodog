using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public Player player; // —сылка на скрипт Player, где хранитс€ здоровье игрока
    public TextMeshProUGUI healthText; // —сылка на компонент TextMeshProUGUI на канвасе

    private void Update()
    {
        // ќбновл€ем текстовое поле здоровь€ с текущим значением healthPoint из скрипта Player
        healthText.text = "Health: " + player.healthPoint.ToString();
    }
}