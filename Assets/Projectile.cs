using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float defaultDamage = 0.1f;
    IDamageable damageableObject;
    int crit;
    bool hasCollide = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollide = true;
            Explode(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyBomb());
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(2f);
        if (!hasCollide)
            Destroy(this.gameObject);
    }

    private void Explode(GameObject player)
    {
        if (player != null)
        {
            HealthManager critText = player.GetComponent<HealthManager>();
            float damage = defaultDamage;
            Rigidbody2D enemyRb = player.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 direction = enemyRb.transform.position - transform.position;
                enemyRb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);

                damageableObject = player.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    crit = 0;
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
