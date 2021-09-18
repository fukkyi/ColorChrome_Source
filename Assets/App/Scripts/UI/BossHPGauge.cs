using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGauge : HpGauge
{
    [SerializeField]
    private Text bossNameText = null;
    [SerializeField]
    private Enemy targetEnemy = null;

    private string bossName = "ボスキノコ";

    private void Update()
    {
        if (targetEnemy == null || !gameObject.activeSelf) return;

        Debug.Log(targetEnemy.GetCurrentHp());
        SetFillAmountByHp(targetEnemy.GetMaxHp(), targetEnemy.GetCurrentHp());

        if (targetEnemy.GetCurrentHp() <= 0)
        {
            HideGauge();
        }
    }

    public void ShowGauge()
    {
        bossNameText.text = bossName;
        gameObject.SetActive(true);
    }

    public void HideGauge()
    {
        gameObject.SetActive(false);
    }
}
