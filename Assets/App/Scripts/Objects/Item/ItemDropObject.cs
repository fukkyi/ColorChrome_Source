using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropObject : MonoBehaviour
{
    [SerializeField]
    protected ItemType[] dropItemTable = new ItemType[0];
    [SerializeField]
    protected int itemCount = 1;
    [SerializeField]
    protected float dropForce = 10.0f;

    /// <summary>
    /// アイテムをドロップさせる
    /// </summary>
    public void DropItem()
    {
        for(int i = 0; i < itemCount; i++)
        {
            DropItemOnce();
        }
    }

    /// <summary>
    /// 一回アイテムをドロップする
    /// </summary>
    private void DropItemOnce()
    {
        ItemType dropItemType = LotteryItem();
        switch (dropItemType)
        {
            case ItemType.RangeUp:
                GameSceneManager.Instance.ColorFlakeBluePool.GetObject<ColorFlake>().DropOfPosition(transform.position, dropForce);
                break;

            case ItemType.AttackUp:
                GameSceneManager.Instance.ColorFlakeRedPool.GetObject<ColorFlake>().DropOfPosition(transform.position, dropForce);
                break;

            case ItemType.HealingUp:
                GameSceneManager.Instance.ColorFlakeGreenPool.GetObject<ColorFlake>().DropOfPosition(transform.position, dropForce);
                break;
        }
    }

    /// <summary>
    /// ドロップテーブルからアイテムを抽選する
    /// </summary>
    /// <returns></returns>
    private ItemType LotteryItem()
    {
        int randElement = Random.Range(0, dropItemTable.Length);

        return dropItemTable[randElement];
    }
}
