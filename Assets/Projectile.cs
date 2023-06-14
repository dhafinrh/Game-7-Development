using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float defaultDamage = 0.1f;
    [SerializeField] private GameObject puddlePrefab;
    IDamageable damageableObject;
    int crit;
    bool hasCollide = false;
    SpriteRenderer projectile;

    public float DefaultDamage { get => defaultDamage; set => defaultDamage = value; }

    private void OnEnable()
    {
        StartCoroutine(DestroyBomb());
        projectile = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (projectile != null)
                projectile.enabled = false;

            hasCollide = true;

            if (enemyType == EnemyType.Bug)
                BugExplode(collision.gameObject);
            else if (enemyType == EnemyType.Botol)
                PuddleSpawn(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.projectile);
        }
    }

    IEnumerator DestroyBomb()
    {
        yield return new WaitForSeconds(2f);
        if (!hasCollide)
            Destroy(this.projectile);
    }

    private void BugExplode(GameObject player)
    {
        if (player != null)
        {
            HealthManager critText = player.GetComponent<HealthManager>();
            Rigidbody2D enemyRb = player.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 direction = enemyRb.transform.position - transform.position;
                enemyRb.AddForce(direction.normalized * explosionForce, ForceMode2D.Impulse);

                damageableObject = player.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    crit = 0;
                    damageableObject.OnHit(defaultDamage, crit);
                }
            }
        }
        else
        {
            Destroy(this.projectile);
        }
    }

    private void PuddleSpawn(Transform playerPos)
    {
        Instantiate(puddlePrefab, playerPos.transform.position, Quaternion.identity);
    }
}
