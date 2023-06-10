using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour, IDamageable
{
    Animator animator;
    Rigidbody2D rb;
    Collider2D enemyCollider;
    public bool Invincible
    {
        get { return invincible; }
        set
        {
            invincible = value;

            if (invincible == true)
            {
                invincibleTimeElapsed = 0f;
            }
        }
    }
    bool invincible;
    float invincibleTimeElapsed = 0f;
    public float maxHealth;
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Dead();
            }
            else
            {
                Damage();
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] private bool isInvincibleEnabled = false;
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private float health;

    [SerializeField] private GameObject dmgTextPrefab;
    [SerializeField] private TMP_Text dmgText;
    [SerializeField] private string colorHex;
    [SerializeField] private float dropChance;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private UnityEvent<float> OnStart;
    [SerializeField] UnityEvent<float> OnHitUpdate;
    [SerializeField] UnityEvent<float> OnDie;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        OnStart.Invoke(Health);
        maxHealth = health;
    }

    private void FixedUpdate()
    {
        if (invincible)
        {
            invincibleTimeElapsed += Time.deltaTime;

            if (invincibleTimeElapsed > invincibleTime)
            {
                invincible = false;
            }
        }
    }

    public void Dead()
    {
        enemyCollider.enabled = false;
        animator.SetTrigger("Dead");

        dropChance = 0.5f;

        if (Random.value <= dropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Damage()
    {
        animator.SetTrigger("Damage");
    }

    public void RemoveEnemy()
    {
        Destroy(this.gameObject);
    }

    public void OnHit(float damage, Vector2 knockBack)
    {
        if (!invincible)
        {
            Health -= damage;
            if (Health > 0)
                OnHitUpdate.Invoke(Health);
            else if (Health <= 0)
                OnDie.Invoke(Health);
            rb.AddForce(knockBack, ForceMode2D.Impulse);
            TextDamage(damage, 0);

            if (isInvincibleEnabled)
            {
                invincible = true;
            }
        }
    }

    public void OnHit(float damage, int crit)
    {
        if (!invincible)
        {
            Health -= damage;
            OnHitUpdate.Invoke(Health);
            TextDamage(damage, crit);

            if (isInvincibleEnabled)
            {
                invincible = true;
            }
        }
    }

    private void TextDamage(float damageAmount, int crit)
    {
        RectTransform textTransform = Instantiate(dmgTextPrefab).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        Canvas canvas = GameObject.FindWithTag("GameplayUI").GetComponent<Canvas>();
        textTransform.SetParent(canvas.transform);

        TMP_Text textComponent = textTransform.GetComponent<TMP_Text>();
        textComponent.text = damageAmount.ToString();

        if (crit == 1)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(colorHex, out color))
            {
                textComponent.color = color;
            }
            else
            {
                textComponent.color = Color.white;
            }
            textComponent.fontSize = 70f;
        }
        else
        {
            textComponent.color = Color.white;
        }
    }

    public void OnHitUpdateUI()
    {
        OnHitUpdate.Invoke(Health);
        Debug.Log("UpdateHealthBar");
    }
}