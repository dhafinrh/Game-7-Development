using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowAbleUI : MonoBehaviour
{
    [SerializeField] private float cooldownDuration;
    [SerializeField] private Image coolDownImg;
    [SerializeField] private TMP_Text cooldownText;
    private float currentCooldown = 0;

    private void Update()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0f)
                currentCooldown = 0f;

            coolDownImg.fillAmount = 1f - (currentCooldown / cooldownDuration);

            cooldownText.text = currentCooldown.ToString("F1");
        }
        else
        {
            cooldownText.enabled = false;
        }
    }

    public void StartCooldown()
    {
        currentCooldown = cooldownDuration;
        cooldownText.enabled = true;
    }
}
