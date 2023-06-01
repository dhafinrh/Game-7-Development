using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private Image healthBackground;
    [SerializeField] private Image healthBar;
    private Camera mainCamera;

    public void MaxHealth(float health)
    {
        maxHealth = health;
    }

    public void UpdateHealthBar(float Health)
    {
        healthBar.fillAmount = Health / maxHealth;
    }
}
