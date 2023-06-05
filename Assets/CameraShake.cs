using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float initialAmplitude;
    private float initialFreq;
    private float shakeTimer;
    private float shakeTimerTotal;
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        cinemachineBasicMultiChannelPerlin =
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }

    public void ShakeEffect(float freq, float amplitude, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = freq;
        initialAmplitude = amplitude;
        initialFreq = freq;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(initialAmplitude, 0f, 1 - ((shakeTimer / shakeTimerTotal)));

                cinemachineBasicMultiChannelPerlin.m_FrequencyGain =
                Mathf.Lerp(initialFreq, 0f, 1 - ((shakeTimer / shakeTimerTotal)));

            }
        }
    }

    public void BattleMode()
    {
        StartCoroutine(TransformOrthographicSize(0.9f, 1.28f, 1f));
    }

    public void WalkMode()
    {
        StartCoroutine(TransformOrthographicSize(1.28f, 0.9f, 1f));
    }

    private IEnumerator TransformOrthographicSize(float startValue, float endValue, float transitionTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime)
        {
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / transitionTime);
            cinemachineVirtualCamera.m_Lens.OrthographicSize = currentValue;
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        cinemachineVirtualCamera.m_Lens.OrthographicSize = endValue;
    }

}
