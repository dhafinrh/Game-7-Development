using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class AnimationManager : MonoBehaviour
{
    private CanvasGroup UIPanel;
    private Tween fadeTween;

    private void OnEnable()
    {
        UIPanel = GetComponent<CanvasGroup>();
        UIPanel.alpha = 0;
        UIPanel.interactable = false;
        UIPanel.blocksRaycasts = false;
        Debug.Log("Jalan");
        UIPanel.transform.position = new Vector3(UIPanel.transform.position.x - 100, UIPanel.transform.position.y, UIPanel.transform.position.z);
    }

    public void FadePanel(bool value)
    {
        if (value)
        {
            Fade(1f, 0.5f, value);
        }
        else
        {
            Fade(0f, 0.5f, value);
        }
    }

    private void FadeMain(float endValue, float duration, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }
        fadeTween = UIPanel.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    private void Fade(float endValue, float duration, bool direction)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        if (direction)
        {
            fadeTween = UIPanel.transform.DOMoveX(UIPanel.transform.position.x + 100, duration).SetEase(Ease.OutExpo);
            fadeTween = UIPanel.DOFade(endValue, duration).OnComplete(() =>
        {
            UIPanel.interactable = true;
            UIPanel.blocksRaycasts = true;
        });

        }
        else
        {
            fadeTween = UIPanel.transform.DOMoveX(UIPanel.transform.position.x - 100, duration).SetEase(Ease.OutExpo);
            fadeTween = UIPanel.DOFade(endValue, duration).OnComplete(() =>
        {
            UIPanel.interactable = false;
            UIPanel.blocksRaycasts = false;
        });

        }
    }
}
