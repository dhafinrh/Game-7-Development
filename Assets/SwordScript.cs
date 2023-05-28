using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] private float damage = 3;
    Vector3 rightAttackOffset;
    Collider2D swordCollider;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
        swordCollider = GetComponent<Collider2D>();
    }

    public void AttackRight()
    {
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.SendMessage("OnHit", damage);
            
            // EnemyScript enemyScript = other.GetComponent<EnemyScript>();

            // if (enemyScript != null)
            // {
            //     enemyScript.Health -= damage;
            // }
        }
    }
}
