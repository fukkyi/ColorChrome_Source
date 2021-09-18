using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    RangeUp,
    AttackUp,
    HealingUp,
    ItemTypeMax,
}

public class ItemGauge : MonoBehaviour
{

    // 倍率のみ使う場合はこっち（いらない場合は消す）
    public float RangeMagnification { get { return magnifications[(int)ItemType.RangeUp]; } }
    public float AttackMagnification { get { return magnifications[(int)ItemType.AttackUp]; } }
    public float HealingMagnification { get { return magnifications[(int)ItemType.HealingUp]; } }

    // レベルのみ使う場合はこっち
    public int RangeLevel { get { return nowLevel[(int)ItemType.RangeUp]; } }
    public int AttackLevel { get { return nowLevel[(int)ItemType.AttackUp]; } }
    public int HealingLevel { get { return nowLevel[(int)ItemType.HealingUp]; } }
    
    private readonly int maxLavel = 5;

    [SerializeField] private Image[] gaugeImage = new Image[(int)ItemType.ItemTypeMax];
    [SerializeField] private Text[] levelText = new Text[(int)ItemType.ItemTypeMax];
    [SerializeField] private float updateSpeed = 1;
    [SerializeField] private float magUp = 1.5f;

    private int[] nowLevel = new int[(int)ItemType.ItemTypeMax];
    private float[] gaugeValue = new float[(int)ItemType.ItemTypeMax];
    private float[] maxGaugeValue = new float[(int)ItemType.ItemTypeMax];
    private float[] magnifications = new float[(int)ItemType.ItemTypeMax] { 1f, 1f, 1f };


    
    #region Initialize
    private void Start()
    {
        GaugeInit();
    }

    private void GaugeInit()
    {
        for (int i = 0; i < gaugeImage.Length; i++)
        {
            nowLevel[i] = 1;
            gaugeImage[i].fillAmount = 0;
            gaugeValue[i] = 0;
            maxGaugeValue[i] = 100;
        }

        for (int i = 0; i < levelText.Length; i++)
        {
            levelText[i].text = nowLevel[i].ToString();
        }
    }
    #endregion


    public void ItemGaugeUpdate(ItemType type, float value)
    {
        var t = (int)type;
        if (nowLevel[t] == maxLavel) { return; }
        gaugeValue[t] += value;
        if (maxGaugeValue[t] <= gaugeValue[t]) { LevelUp(type); }
        else { StartCoroutine(GaugeUpdate(type)); }
    }

    private void LevelUp(ItemType type)
    {
        var t = (int)type;
        nowLevel[t]++;
        if (nowLevel[t] != maxLavel)
        {
            var n = gaugeValue[t] - maxGaugeValue[t];
            gaugeValue[t] = 0 + n;
            maxGaugeValue[t] = maxGaugeValue[t] * 1.5f;
            magnifications[t] *= magUp;
            StartCoroutine(LevelUpGaugeUpdate(type));

            AudioManager.Instance.PlaySE("Powerup upgrade 17");
        }
        else
        {
            gaugeValue[t] = maxGaugeValue[t];
            magnifications[t] *= magUp;
            StartCoroutine(LevelUpGaugeUpdate(type));
        }

        levelText[t].text = nowLevel[t].ToString();
    }

    #region Gauge update process

    /// <summary>
    /// ゲージの増加処理
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator GaugeUpdate(ItemType type)
    {
        var t = (int)type;
        while(gaugeImage[t].fillAmount <= gaugeValue[t] / maxGaugeValue[t])
        {
            
            gaugeImage[t].fillAmount += 
            gaugeImage[t].fillAmount <= gaugeValue[t] / maxGaugeValue[t] ? Time.deltaTime * updateSpeed : gaugeValue[t] / maxGaugeValue[t];
            yield return null;
        }
    }

    /// <summary>
    /// レベルアップ時のゲージ増加処理
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator LevelUpGaugeUpdate(ItemType type)
    {
        var t = (int)type;

        while(gaugeImage[t].fillAmount < 1)
        {
            gaugeImage[t].fillAmount += Time.deltaTime + updateSpeed;
            yield return null;
        }
        gaugeImage[t].fillAmount = 0;
        while (gaugeImage[t].fillAmount <= gaugeValue[t] / maxGaugeValue[t])
        {

            gaugeImage[t].fillAmount +=
            gaugeImage[t].fillAmount <= gaugeValue[t] / maxGaugeValue[t] ? Time.deltaTime * updateSpeed: gaugeValue[t] / maxGaugeValue[t];
            yield return null;
        }
    }
    #endregion
}
