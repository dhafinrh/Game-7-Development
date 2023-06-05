using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] private float damage = 3;
    [SerializeField] private float knockbackForce = 500;
    Vector3 rightAttackOffset;
    Collider2D swordCollider;

    [Header("Camera Properties")]
    [SerializeField] private float amplitude;
    [SerializeField] private float frequention;
    [SerializeField] private float shakeTime;
    [SerializeField] private float hitback;
    private bool inBattle;
    bool enemyDetected = false;

    private void Start()
    {
        swordCollider = GetComponent<Collider2D>();
        rightAttackOffset = transform.localPosition;
        WalkMode();
    }

    public void AttackRight()
    {
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void ShakeAttack()
    {
        CameraShake.Instance.ShakeEffect(frequention, amplitude, shakeTime);
    }
    public void BattleMode()
    {
        CameraShake.Instance.BattleMode();
    }

    public void WalkMode()
    {
        CameraShake.Instance.WalkMode();
    }

    void CheckForEnemies()
    {
        WalkMode();
        enemyDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damagAbleObject = other.GetComponent<IDamageable>();
        if (other.CompareTag("Enemy"))
        {
            if (!enemyDetected)
            {
                BattleMode();
            }

            CancelInvoke("CheckForEnemies");
            enemyDetected = true;
            Invoke("CheckForEnemies", 10);

            if (damagAbleObject != null)
            {
                ShakeAttack();
                Vector3 parent = gameObject.GetComponentInParent<Transform>().position;
                Vector2 direction = (Vector2)(other.gameObject.transform.position - parent).normalized;
                Vector2 knockBack = direction * knockbackForce;

                damagAbleObject.OnHit(damage, knockBack);
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDamagable");
            }
        }

        if (other.CompareTag("Bomb"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.position * hitback, ForceMode2D.Impulse);
        }
    }
}
