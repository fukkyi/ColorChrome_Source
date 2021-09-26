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

    // ”{—¦‚Ì‚İg‚¤ê‡‚Í‚±‚Á‚¿i‚¢‚ç‚È‚¢ê‡‚ÍÁ‚·j
    public float RangeMagnification { get { return magnifications[(int)ItemType.RangeUp]; } }
    public float AttackMagnification { get { return magnifications[(int)ItemType.AttackUp]; } }
    public float HealingMagnification { get { return magnifications[(int)ItemType.HealingUp]; } }

    // ƒŒƒxƒ‹‚Ì‚İg‚¤ê‡‚Í‚±‚Á‚¿
    public int RangeLevel { get { return nowLevel[(int)ItemType.RangeUp]; } }
    public int AttackLevel { get { return nowLevel[(int)ItemType.AttackUp]; } }
    public int HealingLevel { get { return nowLevel[(int)ItemType.HealingUp]; } }
    
    private const int MaxLavel = 5;

    [SerializeField] private Image[] gaugeImage = new Image[(int)ItemType.ItemTypeMax];
    [SerializeField] private Text[] levelText = new Text[(int)ItemType.ItemTypeMax];
    [SerializeField] private float updateSpeed = 1;
    [SerializeField] private float magUp = 1.5f;

    private int[] nowLevel = new int[(int)ItemType.ItemTypeMax];
    private float[] gaugeValue = new float[(int)ItemType.ItemTypeMax];
    private float[] maxGaugeValue = new float[(int)ItemType.ItemTypeMax];
    private float[] magnifications = new float[(int)ItemType.ItemTypeMax] { 1f, 1f, 1f };
    private string[] levelCharacters = new string[MaxLavel] {"‡T", "‡U", "‡V", "‡W", "‡X"};

    
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
            levelText[i].text = levelCharacters[nowLevel[i] - 1];
        }
    }
    #endregion

    public float GetGaugeTotalValue(ItemType type)
    {
        float value = 0;
        float valueRate = 1.0f;

        for(int i = 1; i < nowLevel[(int)type]; i++)
        {
            value += 100 * valueRate;
            valueRate *= 1.5f;
        }

        value += gaugeValue[(int)type];

        return value;
    }

    public void ItemGaugeUpdate(ItemType type, float value, bool playSound = true)
    {
        var t = (int)type;
        if (nowLevel[t] == MaxLavel) { return; }
        gaugeValue[t] += value;

        if (maxGaugeValue[t] <= gaugeValue[t]) {
            while (nowLevel[t] != MaxLavel && maxGaugeValue[t] <= gaugeValue[t]) {
                LevelUp(type, playSound);
            }
        }
        else {
            StartCoroutine(GaugeUpdate(type));
        }
    }

    private void LevelUp(ItemType type, bool playSound = true)
    {
        var t = (int)type;
        nowLevel[t]++;
        if (nowLevel[t] != MaxLavel)
        {
            var n = gaugeValue[t] - maxGaugeValue[t];
            gaugeValue[t] = 0 + n;
            maxGaugeValue[t] = maxGaugeValue[t] * 1.5f;
            magnifications[t] *= magUp;
            StartCoroutine(LevelUpGaugeUpdate(type));

            if (playSound)
            {
                AudioManager.Instance.PlaySE("Powerup upgrade 17");
            }
        }
        else
        {
            gaugeValue[t] = maxGaugeValue[t];
            magnifications[t] *= magUp;
            StartCoroutine(LevelUpGaugeUpdate(type));
        }

        levelText[t].text = levelCharacters[nowLevel[t] - 1];
    }

    #region Gauge update process

    /// <summary>
    /// ƒQ[ƒW‚Ì‘‰Áˆ—
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
    /// ƒŒƒxƒ‹ƒAƒbƒv‚ÌƒQ[ƒW‘‰Áˆ—
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
