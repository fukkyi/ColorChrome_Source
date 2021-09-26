using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayableCalculator : MonoBehaviour
{
    [SerializeField]
    private GrayableObject[] grayableObjects = null;
    [SerializeField]
    private int inkAddRate = 20;

    private int totalGrayAmount = 0;
    private int currentGrayAmount = 0;
    private int currentInkValue = 0;

    public float GrayableRate { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalGrayAmount = ClacTotalGrayAmount();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrayAmount();
    }
    
    /// <summary>
    /// ステージの灰色量を更新する
    /// </summary>
    private void UpdateGrayAmount()
    {
        int beforeGrayAmount = currentGrayAmount;
        int beforeInkValue = currentInkValue;

        currentGrayAmount = ClacCurrentGrayAmount();
        currentInkValue = currentGrayAmount / inkAddRate;

        int diffGrayAmount = currentGrayAmount - beforeGrayAmount;
        int diffInkValue = currentInkValue - beforeInkValue;

        GrayableRate = (float)currentGrayAmount / totalGrayAmount;

        // インク量を変化させる
        GameSceneUIManager.Instance.InkGauge.AddInk(Mathf.Max(diffInkValue, 0));
    }

    /// <summary>
    /// ステージの総灰色量を計算する
    /// </summary>
    /// <returns></returns>
    private int ClacTotalGrayAmount()
    {
        int grayAmount = 0;
        foreach(GrayableObject grayable in grayableObjects)
        {
            grayAmount += grayable.MaxGrayAmount;
        }

        return grayAmount;
    }

    /// <summary>
    /// 現在の灰色量を計算する
    /// </summary>
    /// <returns></returns>
    private int ClacCurrentGrayAmount()
    {
        int grayAmount = 0;
        foreach (GrayableObject grayable in grayableObjects)
        {
            grayAmount += grayable.CurrentGrayAmount;
        }

        return grayAmount;
    }

    /// <summary>
    /// GrayableObjectsを子オブジェクトから自動で設定する
    /// </summary>
    [ContextMenu("SetGrayableByChilds")]
    private void SetGrayableObjectsByChilds()
    {
        grayableObjects = GetComponentsInChildren<GrayableObject>();
    }
}
