using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointObject : MonoBehaviour
{
    [SerializeField]
    private int missionCount = 0;

    public void SetThisCheckPoint()
    {
        // �`�F�b�N�|�C���g���A���ɃN���A�����~�b�V�����̏ꍇ�̓`�F�b�N�|�C���g���Z�b�g���Ȃ�
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
