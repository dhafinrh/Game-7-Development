using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private string text;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] bool isBarrier;
    public UnityEvent OnCutsceneStart;
    public UnityEvent OnCutsceneEnd;
    public UnityEvent OnWarningStart;
    public UnityEvent OnWarningEnd;
    bool isDisplayed;

    private void OnEnable()
    {
        dialogText.text = text.ToString();
    }

    private void Update()
    {
        if (isBarrier)
        {
            bool allEnemiesDead = true;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    allEnemiesDead = false;
                    break;
                }
            }

            if (allEnemiesDead && !isDisplayed)
            {
                dialogText.text = text.ToString();
                boxCollider2D.isTrigger = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDisplayed)
        {
            if (collision.tag == "Player")
            {
                EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript>();
                foreach (EnemyScript enemy in enemies)
                {
                    enemy.enabled = false;
                }
                OnCutsceneStart.Invoke();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDisplayed)
        {
            if (collision.tag == "Player")
            {
                EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript>();
                foreach (EnemyScript enemy in enemies)
                {
                    enemy.enabled = true;
                }
                OnCutsceneEnd.Invoke();
                isDisplayed = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            OnWarningStart.Invoke();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            OnWarningEnd.Invoke();
    }
}
