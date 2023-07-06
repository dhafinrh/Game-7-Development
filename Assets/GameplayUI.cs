using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private Image healthBackground;
    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text currentHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healCoolDownImg;
    [SerializeField] private TMP_Text healCooldownText;
    [SerializeField] private TMP_Text totalCoinText;
    [SerializeField] private TMP_Text blueText;
    [SerializeField] private TMP_Text greenText;
    [SerializeField] private TMP_Text yellowText;
    [SerializeField] private TMP_Text redText;
    [SerializeField] private Image healAvailable;
    private float healCooldownDuration = 0;
    private float currentHealCooldown = 0;
    private Camera mainCamera;
    private int totalCoin;

    public void OnEnable()
    {
        //totalCoin = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinCount(totalCoin);
        maxHealthText.text = maxHealth.ToString();
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
        currentHealth.text = Health.ToString();
    }

    public void OnDie(float Health)
    {
        healthBar.fillAmount = 0;
    }

    public void UpdateCoinCount(int currentCoin)
    {
        totalCoinText.text = currentCoin.ToString();

        if (currentCoin >= 2)
            healAvailable.color = Color.white;
        else
            healAvailable.color = Color.black;
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
                    greenText.color = Color.black;
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
                    yellowText.color = Color.black;
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
                    redText.color = Color.black;
                }
                break;
            default:
                Debug.LogError("Invalid trash ID: " + trashType);
                break;
        }
    }


}
