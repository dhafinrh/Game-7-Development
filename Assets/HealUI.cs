using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealUI : MonoBehaviour
{
    [SerializeField] private Image healCoolDownImg;
    [SerializeField] private TMP_Text healCooldownText;
    private float healCooldownDuration = 0;
    private float currentHealCooldown = 0;
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
}
