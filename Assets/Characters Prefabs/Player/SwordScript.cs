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

    public void ShakeAttack()
    {
        CameraShake.Instance.ShakeEffect(frequention, amplitude, shakeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damagAbleObject = other.GetComponent<IDamageable>();
        if (other.CompareTag("Enemy"))
        {
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
    }
}
