using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float damage = 1;
    [SerializeField] private float knockBackForce = 10f;
    [SerializeField] private DetectPlayer detectPlayer;
    [SerializeField] private float movSpeed;
    [SerializeField] private float avoidForce = 10f;
    [SerializeField] private float minDistance;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public EnemyType EnemyType { get => enemyType; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (detectPlayer.detectedObj.Count > 0)
        {
            bool avoidBombs = detectPlayer.detectedObj.Exists(obj => obj.GetComponent<BombScript>() != null);
            Vector2 playerDirection = (detectPlayer.detectedObj[0].transform.position - transform.position).normalized;
            Debug.Log("avoid : " + avoidBombs);

            if (this.enemyType == EnemyType.Bug)
            {
                float distance = Vector2.Distance(transform.position, detectPlayer.detectedObj[0].transform.position);
                if (avoidBombs)
                {
                    Vector2 bombDirection = (detectPlayer.detectedObj.Find(obj => obj.GetComponent<BombScript>() != null).transform.position - transform.position).normalized;

                    // Check the angle between the bomb direction and the enemy's forward direction
                    float angle = Vector2.SignedAngle(playerDirection, bombDirection);

                    // Move in the perpendicular axis based on the angle
                    if (Mathf.Abs(angle) > 90f)
                    {
                        rb.AddForce(new Vector2(-bombDirection.y, bombDirection.x) * avoidForce * Time.deltaTime, ForceMode2D.Force);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(bombDirection.y, -bombDirection.x) * avoidForce * Time.deltaTime, ForceMode2D.Force);
                    }
                }

                if (distance > minDistance)
                {
                    rb.AddForce(playerDirection * (movSpeed * 3f) * Time.deltaTime);
                }
                else if (distance < minDistance - 0.1f)
                {
                    rb.AddForce(playerDirection * -(movSpeed * 3f) * Time.deltaTime);
                }
                else
                {
                    rb.velocity = Vector2.zero; // Stop the enemy's movement by setting its velocity to zero
                }
            }
            else
            {
                rb.AddForce(playerDirection * movSpeed * Time.deltaTime);
            }


            if (playerDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (playerDirection.x < 0)
            {
                spriteRenderer.flipX = true;
            }
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