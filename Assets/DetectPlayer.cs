using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public List<Collider2D> detectedObj = new List<Collider2D>();
    [SerializeField] private Collider2D detectCol;
    void Start()
    {
        detectCol.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            detectedObj.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            detectedObj.Remove(other);
    }
}
