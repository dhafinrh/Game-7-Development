using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] float timeToLive = 0.5f;
    [SerializeField] float floatSpeed = 500f;
    [SerializeField] Vector3 floatDirection = new Vector3(0, 1, 0);
    TMP_Text dmgText;
    RectTransform rTransform;
    Color startingColor;
    float timeElapsed = 0f;

    private void Start()
    {
        dmgText = GetComponent<TMP_Text>();
        rTransform = GetComponent<RectTransform>();
        startingColor = dmgText.color;
    }

    private void Update()
    {
        rTransform.position += floatDirection * floatSpeed * Time.deltaTime;

        dmgText.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1-(timeElapsed/timeToLive));

        if (timeElapsed <= timeToLive)
            timeElapsed += Time.deltaTime;

        else if (timeElapsed > timeToLive)
            Destroy(gameObject);

    }
}

