using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class FadeableUI : MonoBehaviour
{
    [SerializeField]
    private bool unScaleTime = false;
    [SerializeField, Range(0, 1.0f)]
    private float fadeInAlpha = 1.0f;
    [SerializeField, Range(0, 1.0f)]
    private float fadeOutAlpha = 0;
    [SerializeField]
    private float fadeInTime = 1.0f;
    [SerializeField]
    private float fadeOutTime = 1.0f;
    [SerializeField]
    private AnimationCurve fadeCurve = null;

    private CanvasGroup canvasGroup = null;
    private Action onFadeFinishAction = null;

    private float startAlpha = 0;
    private float finishAlpha = 0;
    private float currentFadeTime = -1.0f;
    private float fadeTime = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        float currentAlpha = canvasGroup.alpha;

        if (currentFadeTime < 0) return;
        // ���݂̃A���t�@�l���ڕW�̃A���t�@�l�������珈�������Ȃ�
        if (currentAlpha == finishAlpha) return;

        currentFadeTime = Mathf.Clamp(currentFadeTime += TimeUtil.GetDeltaTime(unScaleTime), 0, fadeTime);

        float fadeAmount = fadeCurve.Evaluate(currentFadeTime / fadeTime);
        currentAlpha = Mathf.Lerp(startAlpha, finishAlpha, fadeAmount);

        canvasGroup.alpha = currentAlpha;
        // �t�F�[�h�I�����A�A�N�V����������ꍇ�͎��s����
        if (onFadeFinishAction != null && currentFadeTime == fadeTime)
        {
            onFadeFinishAction.Invoke();
        }
    }

    [ContextMenu("FadeIn")]
    /// <summary>
    /// �t�F�[�h�C��������
    /// </summary>
    /// <param name="time"></param>
    /// <param name="onFinish"></param>
    public void FadeIn(float time = default, Action onFinish = null)
    {
        float fadeLength = time == default ? fadeInTime : time;
        SetFade(fadeLength, fadeInAlpha, onFinish);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    [ContextMenu("FadeOut")]
    /// <summary>
    /// �t�F�[�h�A�E�g������
    /// </summary>
    /// <param name="time"></param>
    /// <param name="onFinish"></param>
    public void FadeOut(float time = default, Action onFinish = null)
    {
        float fadeLength = time == default ? fadeOutTime : time;
        SetFade(fadeLength, fadeOutAlpha, onFinish);

        canvasGroup.interactable = false;
    }

    /// <summary>
    /// �t�F�[�h�����邽�߂̒l���Z�b�g����
    /// </summary>
    /// <param name="time"></param>
    /// <param name="fadeAlpha"></param>
    /// <param name="onFinish"></param>
    private void SetFade(float time, float fadeAlpha, Action onFinish = null)
    {
        fadeTime = time;
        currentFadeTime = 0;

        startAlpha = canvasGroup.alpha;
        finishAlpha = fadeAlpha;

        onFadeFinishAction = onFinish;
    }
}
