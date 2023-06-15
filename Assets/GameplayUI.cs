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
    [SerializeField] private TMP_Text blueText;
    [SerializeField] private TMP_Text greenText;
    [SerializeField] private TMP_Text yellowText;
    [SerializeField] private TMP_Text redText;
    private float healCooldownDuration = 0;
    private float currentHealCooldown = 0;
    private Camera mainCamera;
    private int totalCoin;

    public void OnEnable()
    {
        //totalCoin = PlayerPrefs.GetInt("TotalCoins", 0);
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
        healthBar.fillAmount = Health / maxHealth;
    }

    public void OnDie(float Health)
    {
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

    public void UpdateTrashCount(TrashType trashType, int amount, int minimumAmount)
    {
        switch (trashType)
        {
            case TrashType.Green:
                greenText.text = amount.ToString();
                if (amount >= minimumAmount)
                {
                    greenText.color = Color.green;
                }
                else
                {
                    greenText.color = Color.white;
                }
                break;
            case TrashType.Yellow:
                yellowText.text = amount.ToString();
                if (amount >= minimumAmount)
                {
                    yellowText.color = Color.green;
                }
                else
                {
                    yellowText.color = Color.white;
                }
                break;
            case TrashType.Red:
                redText.text = amount.ToString();
                if (amount >= minimumAmount)
                {
                    redText.color = Color.green;
                }
                else
                {
                    redText.color = Color.white;
                }
                break;
            default:
                Debug.LogError("Invalid trash ID: " + trashType);
                break;
        }
    }


}
