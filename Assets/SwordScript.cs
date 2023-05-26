using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] private Collider2D swordCollider;
    [SerializeField] private float damage = 3;
    Vector2 rightAttackOffset;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    public void AttackRight()
    {
        transform.localPosition = rightAttackOffset;
        swordCollider.enabled = true;
    }

    public void AttackLeft()
    {
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        swordCollider.enabled = true;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();

            if (enemyScript != null)
            {
                enemyScript.Health -= damage;
                Debug.Log(enemyScript.Health);
            }
        }
    }
}
