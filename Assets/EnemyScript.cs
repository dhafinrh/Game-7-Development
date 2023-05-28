using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageable
{
    Animator animator;
    Collider2D enemyCollider;
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

    [SerializeField] private float health = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    public void OnHit(float damage)
    {
        Health -= damage;
        Debug.Log("HP : " + Health);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllers playerControllers = other.GetComponent<PlayerControllers>();

            if (playerControllers != null)
            {
                playerControllers.Health -= 1;
            }
        }
    }
}