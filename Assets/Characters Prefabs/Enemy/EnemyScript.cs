using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float damage = 1;
    [SerializeField] private float knockBackForce = 10f;
    [SerializeField] private DetectPlayer detectPlayer;
    [SerializeField] private TMP_Text exclam;
    [SerializeField] private float movSpeed;
    private Animator animator;
    private bool isMoving = false;
    public bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private SpriteRenderer spriteRenderer;
    private float showExclam = 0;
    private bool hasShowedExclam = false;

    [Header("Bug")]
    [SerializeField] private float avoidForce = 10f;
    [SerializeField] private float bugMinDistance;
    [SerializeField] private GameObject projecttile;
    private float shotCooldwon;
    private float nextShot;

    [Header("Kardus")]
    [SerializeField] private float kardusMinDistance;
    [SerializeField] private float strikeForce = 10f;
    private float waitTime;
    private bool isStriking;

    [Header("Bottle")]
    [SerializeField] private GameObject puddleProjectile;

    [Header("Patrol")]
    [SerializeField] private bool enablePatrol = true;
    [SerializeField] private float patrolTime = 3f;
    [SerializeField] private float patrolingSpeed = 75f;
    private bool isPatrolling = false;
    private float patrolTimer = 0f;
    private Vector2 patrolDirection;

    public EnemyType EnemyType { get => enemyType; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        patrolDirection = Random.insideUnitCircle.normalized;

        if (enablePatrol)
        {
            isPatrolling = true;
            patrolTimer = patrolTime;
        }
    }

    private void FixedUpdate()
    {
        if (detectPlayer.detectedObj.Count > 0)
        {
            bool avoidBombs = detectPlayer.detectedObj.Exists(obj => obj.GetComponent<BombScript>() != null);
            playerDirection = (detectPlayer.detectedObj[0].transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, detectPlayer.detectedObj[0].transform.position);

            //For Bug type of enemy
            if (enemyType == EnemyType.Bug)
            {
                //To avoid bombs
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

                //For reaching the player
                if (distance > bugMinDistance)
                {
                    rb.AddForce(playerDirection * (movSpeed * 3f) * Time.deltaTime);
                }
                else if (distance < bugMinDistance - 0.1f)
                {
                    rb.AddForce(playerDirection * -(movSpeed * 2f) * Time.deltaTime);
                }
                else
                {
                    IsMoving = false;
                    rb.velocity = Vector2.zero; // Stop the enemy's movement by setting its velocity to zero
                }
                Rotation();

                if (Time.time > nextShot)
                {
                    GameObject launchedProjetile = Instantiate(projecttile, transform.position, Quaternion.identity);
                    Rigidbody2D rbp = launchedProjetile.GetComponent<Rigidbody2D>();
                    rbp.AddForce(playerDirection * 2, ForceMode2D.Impulse);
                    shotCooldwon = Random.Range(4, 8);
                    nextShot = Time.time + shotCooldwon;
                }
            }
            //For Kardus type of enemy
            else if (enemyType == EnemyType.Kardus)
            //For Bottle type of enemy
            {
                if (distance > kardusMinDistance && !isStriking)
                {
                    IsMoving = true;
                    rb.AddForce(playerDirection * (movSpeed * 3f) * Time.deltaTime);
                }
                else if (distance < kardusMinDistance)
                {
                    if (!isStriking)
                    {
                        isStriking = true;
                        rb.velocity = Vector2.zero;
                        IsMoving = false;
                        PrepareStrike();
                    }
                }
                else
                {
                    IsMoving = false;
                    rb.velocity = Vector2.zero;
                }
            }
            else if (enemyType == EnemyType.Botol)
            {
                if (distance > kardusMinDistance)
                {
                    IsMoving = true;
                    animator.SetBool("Attack", false);
                    rb.AddForce(playerDirection * movSpeed * Time.deltaTime);
                }
                else if (distance < kardusMinDistance)
                {
                    animator.SetBool("Attack", true);
                    rb.AddForce(playerDirection * movSpeed * Time.deltaTime);
                }
                else
                {
                    IsMoving = false;
                    rb.velocity = Vector2.zero;
                }

                if (Time.time > nextShot)
                {
                    GameObject launchedPuddle = Instantiate(puddleProjectile, transform.position, Quaternion.identity);
                    Rigidbody2D rbp = launchedPuddle.GetComponent<Rigidbody2D>();
                    rbp.AddForce(playerDirection * 2, ForceMode2D.Impulse);
                    shotCooldwon = Random.Range(6, 8);
                    nextShot = Time.time + shotCooldwon;
                }
            }

            // Distance check between enemies
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 10f);
            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider != null && enemyCollider.gameObject != gameObject && enemyCollider.CompareTag("Enemy"))
                {
                    Vector2 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;
                    float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);

                    if (distanceToEnemy < 5)
                    {
                        rb.AddForce(-directionToEnemy * 3 * Time.deltaTime, ForceMode2D.Force);
                    }
                }
            }

            Rotation();
        }

        else if (isPatrolling)
        {
            Patrol();
        }
    }


    private void Update()
    {
        if (detectPlayer.EnemyDetected == true)
        {
            if (!hasShowedExclam)
            {
                hasShowedExclam = true;
                showExclam = 2f;
            }
        }

        if (showExclam > 0)
        {
            exclam.enabled = true;
            showExclam -= Time.deltaTime;
        }
        else
        {
            exclam.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy" && this.enemyType != EnemyType.Bug)
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

    private void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f)
        {
            patrolDirection = -patrolDirection;
            patrolTimer = patrolTime;
        }

        rb.AddForce(patrolDirection * patrolingSpeed * Time.deltaTime);
        IsMoving = true;

        Rotation();
    }

    private void Rotation()
    {
        if (playerDirection.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerDirection.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void PrepareStrike()
    {
        if (enabled)
            animator.SetTrigger("doStrike");
    }

    private void Strike()
    {
        if (isStriking)
            rb.MovePosition(rb.position + playerDirection * strikeForce * Time.deltaTime);

        Invoke("DoneStrike", 3f);
    }

    private void DoneStrike()
    {
        isStriking = false;
    }
}