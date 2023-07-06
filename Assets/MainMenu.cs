using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Gameplay");
    }
    
    public void Tutorial(){
        SceneManager.LoadScene("Tutorial");
    }
    
    public void exit(){
        Application.Quit();
    }
}
