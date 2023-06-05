using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowAbleUI : MonoBehaviour
{
    [SerializeField] private Image throwCoolDownImg;
    [SerializeField] private TMP_Text throwCooldownText;
   
    private float throwCooldownDuration = 0;
    private float currentThrowCooldown = 0;
   

    private void Update()
    {
        if (currentThrowCooldown > 0f)
        {
            currentThrowCooldown -= Time.deltaTime;
            if (currentThrowCooldown < 0f)
                currentThrowCooldown = 0f;

            throwCoolDownImg.fillAmount = 1f - (currentThrowCooldown / throwCooldownDuration);

            throwCooldownText.text = currentThrowCooldown.ToString("F1");
        }
        else
        {
            throwCooldownText.enabled = false;
        }
    }

    public void StartThrowCooldown(float cooldown)
    {
        throwCooldownDuration = cooldown;
        currentThrowCooldown = cooldown;
        throwCooldownText.enabled = true;
    }
}
