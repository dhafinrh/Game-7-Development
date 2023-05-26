using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
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
    }

    public void Dead()
    {
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
}
