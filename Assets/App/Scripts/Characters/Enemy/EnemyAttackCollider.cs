using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAttackCollider : AttackCollider
{
    private int targetLayer = LayerMaskUtil.PlayerLayerNumber;

    protected override void CollisionManage(Collider collider)
    {
        base.CollisionManage(collider);

        if (collider.gameObject.layer != targetLayer) return;

        Player playerComponent = collider.GetComponentInParent<Player>();

        // Šù‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚½‚à‚Ì‚È‚ç–³‹‚·‚é
        if (damagedObjectList.Contains(playerComponent.gameObject)) return;

        playerComponent.TakeDamage(damageValue);

        damagedObjectList.Add(playerComponent.gameObject);
    }
}
