using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrashCanScript : MonoBehaviour
{
    [SerializeField] private TrashType trashtype;
    [SerializeField] private Canvas notMatchCanvas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player found");
            PlayerControllers playerControllers = other.GetComponentInChildren<PlayerControllers>();

            TrashScript currentTrashID = playerControllers.CurrentTrashType;

            if (currentTrashID != null)
            {
                if (this.trashtype == currentTrashID.TrashType)
                {
                    Debug.Log(this.trashtype);
                    playerControllers.TrashCollected();
                }
                else
                {
                    notMatchCanvas.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            notMatchCanvas.enabled = false;
    }

    IEnumerator DestroyTrash(GameObject thisTrash)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(thisTrash);
    }
}

