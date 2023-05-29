using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float damage = 1;
    [SerializeField] private float knockBackForce = 10f;
    [SerializeField] private DetectPlayer detectPlayer;
    [SerializeField] private float movSpeed;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (detectPlayer.detectedObj.Count > 0)
        {
            Vector2 direction = (detectPlayer.detectedObj[0].transform.position - transform.position).normalized;

            rb.AddForce(direction * movSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Collider2D collider = other.collider;
            IDamageable damagAble = other.collider.GetComponent<IDamageable>();
            if (damagAble != null)
            {
                Vector2 direction = (Vector2)(other.transform.position - transform.position).normalized;
                Vector2 knockBack = direction * knockBackForce;

                damagAble.OnHit(damage, knockBack);
            }
        }
    }
}