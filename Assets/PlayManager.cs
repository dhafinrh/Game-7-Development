using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStart;
    [SerializeField] private PlayerControllers playerControllers;
    public int playerCoin = 0;
    public UnityEvent<int> onUpdateCoin;
    [SerializeField] private bool isCursorVisible;
    [SerializeField] private int greenTrash;
    [SerializeField] private int yellowTrash;
    [SerializeField] private int redTrash;
    [SerializeField] private int minimumGreen;
    [SerializeField] private int minimumYellow;
    [SerializeField] private int minimumRed;
    [SerializeField] private UnityEvent<TrashType, int, int> UpdateTrashText;
    [SerializeField] private UnityEvent OnPause;
    [SerializeField] private UnityEvent OnWin;
    public bool isPausing;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //playerCoin = PlayerPrefs.GetInt("TotalCoins");
    }

    private void Start()
    {
        OnStart.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isCursorVisible == false)
        {
            isCursorVisible = true;
            Cursor.visible = isCursorVisible;
            if (!isPausing)
            {
                OnPause.Invoke();
                Invoke("Pause", 0.5f);
            }

            isPausing = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isCursorVisible == true)
        {
            isCursorVisible = false;
            Cursor.visible = isCursorVisible;
        }

        if (greenTrash >= minimumGreen && yellowTrash >= minimumYellow && redTrash >= minimumRed)
            OnWin.Invoke();

    }

    private void OnEnable()
    {
        CoinScript.onCoinCollected += HandleCoinCollected;
    }

    private void OnDisable()
    {
        CoinScript.onCoinCollected -= HandleCoinCollected;
    }

    public void ShowCursor(bool value)
    {
        isCursorVisible = value;
        Cursor.visible = isCursorVisible;
    }

    public void IsPausing(bool value)
    {
        isPausing = value;
    }

    private void HandleCoinCollected(int value)
    {
        playerCoin += value;

        PlayerPrefs.SetInt("TotalCoins", playerCoin);
        PlayerPrefs.Save();

        onUpdateCoin.Invoke(playerCoin);
    }

    public void HandleTrashCollected(TrashType trashType)
    {
        Debug.Log("HandleTrashCollected");
        switch (trashType)
        {
            case TrashType.Green:
                greenTrash++;
                UpdateTrashText.Invoke(trashType, greenTrash, minimumGreen);
                break;
            case TrashType.Yellow:
                yellowTrash++;
                UpdateTrashText.Invoke(trashType, yellowTrash, minimumYellow);
                break;
            case TrashType.Red:
                redTrash++;
                UpdateTrashText.Invoke(trashType, redTrash, minimumRed);
                break;
            default:
                Debug.LogError("Invalid trash ID: " + trashType);
                break;
        }
    }

    public void Replay()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript>();
        foreach (EnemyScript enemy in enemies)
        {
            enemy.enabled = false;
        }
        playerControllers.enabled = false;
    }

    public void Resume()
    {
        {
            EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.enabled = true;
            }
            playerControllers.enabled = true;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
