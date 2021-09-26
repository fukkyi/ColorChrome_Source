using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkGauge : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImage = null;
    [SerializeField]
    private float maxFillPosY = 0;
    [SerializeField]
    private float minFillPosX = 180;
    /// <summary>
    /// 最大インク量
    /// </summary>
    [SerializeField]
    private int maxInk = 1000;

    private int currentInk = 0;

    public bool IsEnoughInk(int value)
    {
        return value <= currentInk;
    }

    /// <summary>
    /// インク量を追加する
    /// </summary>
    /// <param name="value"></param>
    public void AddInk(int value)
    {
        currentInk = Mathf.Clamp(currentInk + value, 0, maxInk);
        SetFillAmountByInk(maxInk, currentInk);
    }

    /// <summary>
    /// インクを最大にする
    /// </summary>
    public void AddMaxInk()
    {
        AddInk(maxInk);
    }

    /// <summary>
    /// インクゲージの量をインク量から設定する
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public void SetFillAmountByInk(int maxInk, int currentInk)
    {
        float inkRate = (float)currentInk / maxInk;
        // インクの量によってゲージの高さを変える
        gaugeImage.rectTransform.anchoredPosition = Vector3.up * Mathf.Lerp(minFillPosX, maxFillPosY, inkRate);
    }
}
