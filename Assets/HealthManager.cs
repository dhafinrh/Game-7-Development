using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                RectTransform textTransform = Instantiate(dmgText).GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindObjectOfType<Canvas>();
                textTransform.SetParent(canvas.transform);
            }
        }
        get
        {
            return health;
        }
    }

    [SerializeField] private bool isInvincibleEnabled = false;
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private float health = 1;
    [SerializeField] private GameObject dmgText;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
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

            rb.AddForce(knockBack, ForceMode2D.Impulse);

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

            if (isInvincibleEnabled)
            {
                invincible = true;
            }
        }
    }
}