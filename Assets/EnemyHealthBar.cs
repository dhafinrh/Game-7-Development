using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private float maxHealth;
    [SerializeField] private Image healthBackground;
    [SerializeField] private Image healthBar;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Camera mainCamera;

    private void OnEnable()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void MaxHealth(float health)
    {
        maxHealth = health;
    }

    void Update()
    {
        healthBackground.transform.rotation = mainCamera.transform.rotation;
        healthBackground.transform.position = target.position + offset;

        healthBar.transform.rotation = mainCamera.transform.rotation;
        healthBar.transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float Health)
    {
        healthBar.fillAmount = Health / maxHealth;
    }

    public void Disband()
    {
        healthBackground.enabled = false;
        healthBar.enabled = false;
    }
}
