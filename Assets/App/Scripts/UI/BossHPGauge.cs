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

    private void Update()
    {
        if (targetEnemy == null || !gameObject.activeSelf) return;

        SetFillAmountByHp(targetEnemy.GetMaxHp(), targetEnemy.GetCurrentHp());

        if (targetEnemy.GetCurrentHp() <= 0)
        {
            HideGauge();
        }
    }

    public void SetGaugeDetail(Enemy enemy, string name)
    {
        targetEnemy = enemy;
        bossNameText.text = name;
    }

    public void ShowGauge()
    {
        gameObject.SetActive(true);
    }

    public void HideGauge()
    {
        gameObject.SetActive(false);
    }
}
