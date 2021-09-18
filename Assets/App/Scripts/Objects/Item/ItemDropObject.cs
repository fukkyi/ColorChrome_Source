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
    /// �A�C�e�����h���b�v������
    /// </summary>
    public void DropItem()
    {
        for(int i = 0; i < itemCount; i++)
        {
            DropItemOnce();
        }
    }

    /// <summary>
    /// ���A�C�e�����h���b�v����
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
    /// �h���b�v�e�[�u������A�C�e���𒊑I����
    /// </summary>
    /// <returns></returns>
    private ItemType LotteryItem()
    {
        int randElement = Random.Range(0, dropItemTable.Length);

        return dropItemTable[randElement];
    }
}
