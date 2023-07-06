using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;
    private TMP_Text text;
    private Image image;

    private void OnEnable()
    {
        text = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Button>().image;

        ChangeColor(false);
        image.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor(true);
        image.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeColor(false);
        image.enabled = false;
    }

    private void ChangeColor(bool value)
    {
        string enter = "#ffffff";
        string exit = "#000000";

        ColorUtility.TryParseHtmlString(enter, out Color white);
        ColorUtility.TryParseHtmlString(exit, out Color black);

        if (value)
        {
            text.color = white;
        }
        else
        {
            text.color = black;
        }
    }
}
