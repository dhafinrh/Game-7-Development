using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinScript : MonoBehaviour
{
    Animator animator;
    bool coinCollected;
    bool timeout = false;
    CircleCollider2D circleCollider2D;
    public static event System.Action<int> onCoinCollected;
    
    public AudioSource onCollect; 
    public AudioClip AudioClip;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator.SetTrigger("Spawn");
        StartCoroutine(CountDown());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!timeout)
            {
                coinCollected = true;
                StartCoroutine(CoinCollected());

                onCoinCollected?.Invoke(1);
                
                onCollect.PlayOneShot(AudioClip);
            }
            else
            {
                return;
            }
        }
    }

    IEnumerator CoinCollected()
    {
        animator.SetBool("isCollected", true);
        yield return new WaitForSeconds(0.3f);
        Destroy(transform.parent.gameObject);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5f);

        if (!coinCollected)
        {
            timeout = true;
            circleCollider2D.enabled = false;
            animator.SetBool("isCollected", true);
            yield return new WaitForSeconds(0.7f);
            Destroy(transform.parent.gameObject);
        }
    }
}
