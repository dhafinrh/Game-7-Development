using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public int playerCoin = 0;
    public UnityEvent<int> onUpdateCoin;

    private void Start()
    {
        playerCoin = PlayerPrefs.GetInt("TotalCoins");
    }

    private void OnEnable()
    {
        CoinScript.onCoinCollected += HandleCoinCollected;
    }

    private void OnDisable()
    {
        CoinScript.onCoinCollected -= HandleCoinCollected;
    }

    private void HandleCoinCollected(int value)
    {
        playerCoin += value;

        PlayerPrefs.SetInt("TotalCoins", playerCoin);
        PlayerPrefs.Save();

        onUpdateCoin.Invoke(playerCoin);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
