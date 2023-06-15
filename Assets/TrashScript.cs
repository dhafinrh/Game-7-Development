using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashScript : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private List<Sprite> greenTrashSprites;
    [SerializeField] private List<Sprite> yellowTrashSprites;
    [SerializeField] private List<Sprite> redTrashSprites;
    [SerializeField] bool randomize = true;
    private SpriteRenderer thisTrash;

    public TrashType TrashType { get => trashType; }

    private void OnEnable()
    {
        if (randomize)
            trashType = (TrashType)Random.Range(1, 4);

        thisTrash = GetComponentInChildren<SpriteRenderer>();

        switch (trashType)
        {
            case TrashType.Green:
                thisTrash.sprite = greenTrashSprites[Random.Range(0, greenTrashSprites.Count)];
                break;
            case TrashType.Yellow:
                thisTrash.sprite = yellowTrashSprites[Random.Range(0, yellowTrashSprites.Count)];
                break;
            case TrashType.Red:
                thisTrash.sprite = redTrashSprites[Random.Range(0, redTrashSprites.Count)];
                break;
            default:
                Debug.LogError("Invalid trash type: " + trashType);
                break;
        }
    }
}
