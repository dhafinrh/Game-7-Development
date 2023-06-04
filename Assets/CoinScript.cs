using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    Animator animator;
    CircleCollider2D circleCollider2D;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CoinCollected());
        }
    }

    IEnumerator CoinCollected()
    {
        animator.SetBool("isCollected", true);
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);

     }
}
