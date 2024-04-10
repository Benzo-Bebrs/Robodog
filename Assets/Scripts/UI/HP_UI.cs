using UnityEngine;
using TMPro;
using System;

public class HealthDisplay : MonoBehaviour
{
    public Player player; // Reference to the Player script
    public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component on the canvas

    private void Start()
    {
        // Subscribe to the player's health change event
        player.OnHealthChanged += UpdateHealth;

        // Update the initial health value
        UpdateHealth(player.HealthPoint);
    }

    private void UpdateHealth(float health)
    {
        // Update the health text on the canvas
        healthText.text = "Health: " + health.ToString();
    }
}