using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float damage = 1;
    IDamageable damageableObject;
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
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyBomb());
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(1.25f);
        if (!hasCollide)
            Destroy(this.gameObject);
    }

    private void Explode()
    {
        // Apply explosion force to all nearby enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius: explosionRadius);
        foreach (Collider2D collider in colliders)

        {
            if (collider.CompareTag("Enemy"))
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 direction = enemyRb.transform.position - transform.position;
                    enemyRb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);

                    damageableObject = collider.GetComponent<IDamageable>();
                    if (damageableObject != null)
                    {
                        damageableObject.OnHit(damage);
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
