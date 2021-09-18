using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "ColorLevelProportionTable",
  menuName = "ScriptableObject/ColorLevelProportionTable")
]
public class ColorLevelProportionTable : ScriptableObject
{
    private int firstLevelElement = 1;

    [SerializeField]
    private float[] levelProportions = new float[0];

    /// <summary>
    /// 現在のレベルから倍率を取得する
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float GetProportionByLevel(int level)
    {
        level = Mathf.Clamp(level, firstLevelElement, levelProportions.Length + firstLevelElement);

        return levelProportions[level - firstLevelElement];
    }

    /// <summary>
    /// アイテムの種類から倍率を取得する
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public float GetProportionByItemType(ItemType itemType)
    {
        int level = 1;
        switch(itemType)
        {
            case ItemType.RangeUp:
                level = GameSceneUIManager.Instance.ItemGauge.RangeLevel;
                break;
            case ItemType.AttackUp:
                level = GameSceneUIManager.Instance.ItemGauge.AttackLevel;
                break;
            case ItemType.HealingUp:
                level = GameSceneUIManager.Instance.ItemGauge.HealingLevel;
                break;
        }

        return GetProportionByLevel(level);
    }
}
