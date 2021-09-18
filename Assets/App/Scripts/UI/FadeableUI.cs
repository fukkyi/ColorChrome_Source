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
        // 現在のアルファ値が目標のアルファ値だったら処理をしない
        if (currentAlpha == finishAlpha) return;

        currentFadeTime = Mathf.Clamp(currentFadeTime += TimeUtil.GetDeltaTime(unScaleTime), 0, fadeTime);

        float fadeAmount = fadeCurve.Evaluate(currentFadeTime / fadeTime);
        currentAlpha = Mathf.Lerp(startAlpha, finishAlpha, fadeAmount);

        canvasGroup.alpha = currentAlpha;
        // フェード終了時、アクションがある場合は実行する
        if (onFadeFinishAction != null && currentFadeTime == fadeTime)
        {
            onFadeFinishAction.Invoke();
        }
    }

    [ContextMenu("FadeIn")]
    /// <summary>
    /// フェードインさせる
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
    /// フェードアウトさせる
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
    /// フェードさせるための値をセットする
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
