using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
