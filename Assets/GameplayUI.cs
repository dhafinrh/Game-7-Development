using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private Image healthBackground;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healCoolDownImg;
    [SerializeField] private TMP_Text healLeft;
    [SerializeField] private TMP_Text healCooldownText;
    [SerializeField] private TMP_Text totalCoinText;
    private float healCooldownDuration = 0;
    private float currentHealCooldown = 0;
    private Camera mainCamera;
    private int totalCoin;

    public void OnEnable()
    {
        totalCoin = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinCount(totalCoin);
    }

    void Update()
    {
        if (currentHealCooldown > 0f)
        {
            currentHealCooldown -= Time.deltaTime;
            if (currentHealCooldown < 0f)
                currentHealCooldown = 0f;

            healCoolDownImg.fillAmount = 1f - (currentHealCooldown / healCooldownDuration);

            healCooldownText.text = currentHealCooldown.ToString("F1");
        }
        else
        {
            healCooldownText.enabled = false;
        }
    }

    public void StartHealCooldown(float cooldown)
    {
        healCooldownDuration = cooldown;
        currentHealCooldown = cooldown;
        healCooldownText.enabled = true;
    }

    public void MaxHealth(float health)
    {
        maxHealth = health;
    }

    public void UpdateHealthBar(float Health)
    {
        if (Health > 0)
            healthBar.fillAmount = Health / maxHealth;
        else if (Health <= 0)
            healthBar.fillAmount = 0;
    }

    public void UpdateCoinCount(int currentCoin)
    {
        totalCoinText.text = currentCoin.ToString();
    }

    public void UpdateHealCount(int heal)
    {
        healLeft.text = heal.ToString();
    }

}
