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
    [SerializeField] private UnityEvent<float> OnStart;
    [SerializeField] private UnityEvent <float>OnHitUpdate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        OnStart.Invoke(Health);
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
            OnHitUpdate.Invoke(Health);
            rb.AddForce(knockBack, ForceMode2D.Impulse);
            TextDamage(damage);

            if (isInvincibleEnabled)
            {
                invincible = true;
            }
        }
    }

    public void OnHit(float damage)
    {
        if (!invincible)
        {
            Health -= damage;
            OnHitUpdate.Invoke(Health);
            TextDamage(damage);

            if (isInvincibleEnabled)
            {
                invincible = true;
            }
        }
    }

    private void TextDamage(float damageAmount)
    {
        dmgText.text = damageAmount.ToString();
        RectTransform textTransform = Instantiate(dmgTextPrefab).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        Canvas canvas = GameObject.FindWithTag("GameplayUI").GetComponent<Canvas>();
        textTransform.SetParent(canvas.transform);
    }
}