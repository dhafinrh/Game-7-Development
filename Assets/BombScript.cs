using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] private BombType bombType;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float defaultDamage = 1;
    IDamageable damageableObject;
    int crit;
    bool hasCollide = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasCollide = true;
            Explode();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyBomb());
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(1.25f);
        if (!hasCollide)
            Destroy(this.gameObject);
    }

    private void Explode( )
    {
        // Apply explosion force to all nearby enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius: explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyScript enemyScript = collider.GetComponent<EnemyScript>();
                HealthManager critText = collider.GetComponent<HealthManager>();
                float damage = defaultDamage;
                float forceMultiplier;

                // Adjust damage and force based on enemy name
                if (enemyScript.EnemyType== EnemyType.Kardus && bombType == BombType.Water)
                {
                    // Debug.Log("Kardus Ketemu Air");
                    damage *= 5f;
                    forceMultiplier = 2f;
                    crit = 1;
                }
                else if (enemyScript.EnemyType == EnemyType.Botol && bombType == BombType.Fire)
                {
                    //Debug.Log("Botol Ketemu Api");
                    damage *= 5f;
                    forceMultiplier = 2f;
                    crit = 1;

                }
                else if (enemyScript.EnemyType == EnemyType.Bug && bombType == BombType.Bug)
                {
                    damage *= 9f;
                    forceMultiplier = 1.5f;
                    crit = 1;
                }
                else
                {
                    damage = defaultDamage;
                    forceMultiplier = 1f;
                    crit = 0;
                }

                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 direction = enemyRb.transform.position - transform.position;
                    enemyRb.AddForce(direction.normalized * explosionForce * forceMultiplier, ForceMode2D.Impulse);

                    damageableObject = collider.GetComponent<IDamageable>();
                    if (damageableObject != null)
                    {
                        damageableObject.OnHit(damage, crit);
                    }
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
