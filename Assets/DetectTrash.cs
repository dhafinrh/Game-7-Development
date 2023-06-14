// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;


// public class DetectTrash : MonoBehaviour
// {
//     private PlayerControllers playerControllers;
//     private Canvas canvasPickUp;
//     private GameObject grabbedObject = null;
//     private BoxCollider2D detectTrash;

//     private void OnEnable()
//     {
//         detectTrash = GetComponent<BoxCollider2D>();
//         playerControllers = GetComponent<PlayerControllers>();
//     }
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         TrashScript trashScript = other.GetComponent<TrashScript>();
//         if (trashScript != null)
//         {
//             Debug.Log("Trash Found");

//             canvasPickUp = other.gameObject.GetComponentInChildren<Canvas>();
//             if (canvasPickUp != null && grabbedObject == null)
//                 canvasPickUp.enabled = true;
//         }

//         else if (trashScript == null)
//         {
//             Debug.Log("Not a trash");
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (grabbedObject == null)
//         {
//             TrashScript trashScript = other.GetComponent<TrashScript>();
//             if (trashScript != null && canvasPickUp != null)
//                 canvasPickUp.enabled = false;
//         }
//     }

//     private void Update()
//     {
//         if (Keyboard.current.spaceKey.wasReleasedThisFrame && grabbedObject == null)
//         {
//             Debug.Log("Trash Picked");
//             grabbedObject = other.gameObject;
//             grabbedObject.transform.SetParent(transform);
//             grabbedObject.transform.localPosition = playerControllers.GrabPoint.localPosition;
//         }
//         if ((Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && grabbedObject != null)
//         {
//             grabbedObject.transform.SetParent(null);
//             grabbedObject = null;
//         }
//     }
// }
