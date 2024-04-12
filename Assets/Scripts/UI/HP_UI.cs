using UnityEngine;
using System;

public class HealthDisplay : MonoBehaviour
{
    public Player player; 
    private GameObject heartImage; 
    private GameObject shieldImage;

    private void Start()
    {
        heartImage = transform.Find("heart").gameObject;
        shieldImage = transform.Find("shield").gameObject;
        player.OnHealthChanged += UpdateHealth;
        UpdateHealth(player.HealthPoint);
    }

    private void UpdateHealth(float health)
    {
        if (health == 1)
        {
            heartImage.SetActive(true);
            shieldImage.SetActive(false);
        }
        else if (health == 2)
        {
            heartImage.SetActive(true);
            shieldImage.SetActive(true);
        }
        else
        {
            heartImage.SetActive(false);
            shieldImage.SetActive(false);
        }
    }
}