using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image LoadBar;
    [SerializeField] private TMP_Text LoadText;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        LoadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadBar.fillAmount = progressValue;
            LoadText.text = ((int)(progressValue * 100)).ToString() + "%";

            yield return null;
        }
    }
}
