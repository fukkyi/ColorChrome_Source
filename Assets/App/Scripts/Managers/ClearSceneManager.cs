using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClearSceneManager : BaseSceneManager<ClearSceneManager>
{
    [SerializeField]
    private GameObject happyEre = null;
    [SerializeField]
    private GameObject happyKeo = null;
    [SerializeField]
    private Text gameClearText = null;
    [SerializeField]
    private Text continueCountText = null;
    [SerializeField]
    private Text playTimeText = null;
    [SerializeField]
    private Text grayRateText = null;
    [SerializeField]
    private Text happyHintText = null;
    [SerializeField]
    private Image backImage = null;
    [SerializeField]
    private Color happyEndingColor = new Color();
    [SerializeField]
    private Color badEndingColor = new Color();

    private bool isBackedTitle = false;

    private void Start()
    {
        if (GamePlayDataManager.IsValidHappyEnding())
        {
            gameClearText.color = happyEndingColor;
            happyEre.SetActive(true);
            happyKeo.SetActive(true);
            backImage.material.SetFloat("_Threshold", 0);
        }
        else
        {
            gameClearText.color = badEndingColor;
            happyHintText.gameObject.SetActive(true);
            backImage.material.SetFloat("_Threshold", 1);
        }

        float finishTime = Time.realtimeSinceStartup;
        float clearTime = finishTime - GamePlayDataManager.playTime;
        TimeSpan clearTimeSpan = new TimeSpan(0, 0, (int)clearTime);

        continueCountText.text = "コンティニュー回数: " + GamePlayDataManager.continueCount.ToString();
        playTimeText.text = "プレイ時間: " + clearTimeSpan.ToString();
        grayRateText.text = "灰色にした割合: " + (GamePlayDataManager.grayRate * 100.0f).ToString("F2") + "%";
    }

    public void BackTitle()
    {
        if (isBackedTitle) return;

        SceneTransitionManager.Instance.StartTransitionByName(SceneTransitionManager.TitleSceneName);

        isBackedTitle = true;
    }
}
