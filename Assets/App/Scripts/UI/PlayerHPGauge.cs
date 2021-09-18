using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPGauge : HpGauge
{
    private Color normalGaugeColor = Color.green;
    private Color injuredGaugeColor = new Color(1f, 127f / 255f, 39f / 255f);
    private Color dyingGaugeColor = Color.red;

    private float injuredGaugeRate = 0.75f;
    private float dyingGaugeRate = 0.3f;

    // Start is called before the first frame update
    private void Start()
    {
        gaugeImage.color = normalGaugeColor;
    }

    /// <summary>
    /// HPÉQÅ[ÉWÇÃó ÇHPÇ©ÇÁê›íËÇ∑ÇÈ
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public override void SetFillAmountByHp(int maxHp, int currentHp)
    {
        float hpRate = (float)currentHp / maxHp;
        base.SetFillAmountByHp(maxHp, currentHp);

        if (hpRate > injuredGaugeRate)
        {
            gaugeImage.color = normalGaugeColor;
        }
        else if (hpRate > dyingGaugeRate)
        {
            gaugeImage.color = injuredGaugeColor;
        }
        else
        {
            gaugeImage.color = dyingGaugeColor;
        }
    }
}
