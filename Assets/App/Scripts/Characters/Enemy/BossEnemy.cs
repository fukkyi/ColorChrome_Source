using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField]
    protected string bossName = string.Empty;

    protected override void OnDetected()
    {
        base.OnDetected();
        GameSceneUIManager.Instance.BossHPGauge.SetGaugeDetail(this, bossName);
        GameSceneUIManager.Instance.BossHPGauge.ShowGauge();
    }

    protected override void OnLoseSight()
    {
        base.OnLoseSight();
        GameSceneUIManager.Instance.BossHPGauge.HideGauge();
    }
}
