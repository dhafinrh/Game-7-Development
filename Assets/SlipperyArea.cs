using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyArea : MonoBehaviour
{
    Animator animator;
    bool timeout = false;
    CapsuleCollider2D capsuleCollider2D;


    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator.SetTrigger("Spawn");
        StartCoroutine(CountDown());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!timeout)
            {
                PlayerControllers pl = other.GetComponentInChildren<PlayerControllers>();
                pl.ApplySlipperyEffect();
            }
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5f);

        timeout = true;
        capsuleCollider2D.enabled = false;
        animator.SetBool("isDespawn", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);

    }
}
