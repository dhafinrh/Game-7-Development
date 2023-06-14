using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource FootStep; 
    void Update()
    {
        if(Input.GetAxis("Vertical") !=0 || Input.GetAxis("Horizontal") !=0){
            // Debug.Log("move");
            FootStep.enabled = true;
        } else {
            FootStep.enabled = false;
        }
    }
}
