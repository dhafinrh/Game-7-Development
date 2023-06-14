using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public int playerCoin = 0;
    public UnityEvent<int> onUpdateCoin;
    [SerializeField] private bool isCursorVisible;
    [SerializeField] private int blueTrash;
    [SerializeField] private int greenTrash;
    [SerializeField] private int yellowTrash;
    [SerializeField] private int redTrash;
    [SerializeField] private UnityEvent<TrashType,int> UpdateTrashText;


    private void Awake()
    {
        Cursor.visible = isCursorVisible;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        playerCoin = PlayerPrefs.GetInt("TotalCoins");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isCursorVisible == false)
        {
            isCursorVisible = true;
            Cursor.visible = isCursorVisible;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isCursorVisible == true)
        {
            isCursorVisible = false;
            Cursor.visible = isCursorVisible;

        }
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

    public void HandleTrashCollected(TrashType trashType)
    {
        Debug.Log("HandleTrashCollected");
        switch (trashType)
        {
            case TrashType.Green:
                greenTrash++;
                UpdateTrashText.Invoke(trashType, greenTrash);
                Debug.Log("Yang di data Green : " + greenTrash);
                break;
            case TrashType.Yellow:
                yellowTrash++;
                UpdateTrashText.Invoke(trashType, yellowTrash);
                Debug.Log("Yang di data Yellow : " + yellowTrash);
                break;
            case TrashType.Red:
                redTrash++;
                UpdateTrashText.Invoke(trashType, redTrash);
                Debug.Log("Yang di data Red : " + redTrash);
                break;
            default:
                Debug.LogError("Invalid trash ID: " + trashType);
                break;
        }
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
