using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : AttackCollider
{
    [SerializeField]
    private int addInkValue = 20;

    private int targetLayer = LayerMaskUtil.EnemyLayerNumber;

    protected override void CollisionManage(Collider collider)
    {
        base.CollisionManage(collider);

        if (collider.gameObject.layer != targetLayer) return;

        int attackPower = (int)(damageValue * GameSceneManager.Instance.RedLevelTable.GetProportionByItemType(ItemType.AttackUp));
        Enemy enemyComponent = collider.GetComponentInParent<Enemy>();

        // ���Ƀ_���[�W��^�������̂Ȃ疳������
        if (damagedObjectList.Contains(enemyComponent.gameObject)) return;

        enemyComponent.TakeDamage(attackPower);

        damagedObjectList.Add(enemyComponent.gameObject);

        GameSceneUIManager.Instance.InkGauge.AddInk(addInkValue);

        AudioManager.Instance.PlayRandomPitchSE("Staff Hitting (Flesh) 1", transform.position);
    }
}
