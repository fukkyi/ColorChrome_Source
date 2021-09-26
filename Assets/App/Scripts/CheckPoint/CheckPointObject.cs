using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObject : MonoBehaviour
{
    [SerializeField]
    private int missionCount = 0;

    public void SetThisCheckPoint()
    {
        // チェックポイント復帰時にクリアしたミッションの場合はチェックポイントをセットしない
        if (MissionSetter.Instance.IsCheckPointInit) return;

        CheckPoint checkPoint = new CheckPoint();

        checkPoint.playerPos = transform.position;
        checkPoint.playerRot = Quaternion.Inverse(transform.rotation);
        checkPoint.missionCount = missionCount;
        checkPoint.colorAttackValue = GameSceneUIManager.Instance.ItemGauge.GetGaugeTotalValue(ItemType.AttackUp);
        checkPoint.colorHealingValue = GameSceneUIManager.Instance.ItemGauge.GetGaugeTotalValue(ItemType.HealingUp);
        checkPoint.colorRangeValue = GameSceneUIManager.Instance.ItemGauge.GetGaugeTotalValue(ItemType.RangeUp);

        GamePlayDataManager.currentCheckPoint = checkPoint;
    }
}
