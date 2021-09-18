using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour
{
    [SerializeField]
    protected Image gaugeImage = null;

    /// <summary>
    /// HP�Q�[�W�̗ʂ�HP����ݒ肷��
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public virtual void SetFillAmountByHp(int maxHp, int currentHp)
    {
        float hpRate = (float)currentHp / maxHp;
        gaugeImage.fillAmount = hpRate;
    }
}
