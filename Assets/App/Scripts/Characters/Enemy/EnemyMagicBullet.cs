using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicBullet : MagicBullet
{
    /// <summary>
    /// 衝突した際の処理
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionAction(Collision collision)
    {
        base.OnCollisionAction(collision);

        AudioManager.Instance.PlaySE("Buff 1", transform.position);

        if (collision.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        Player playerComponent = collision.gameObject.GetComponent<Player>();

        // 既にダメージを与えたものなら無視する
        if (damagedObjectList.Contains(playerComponent.gameObject)) return;

        playerComponent.TakeDamage(attackPower);
        damagedObjectList.Add(playerComponent.gameObject);
    }

    protected override void PlayCollsionParticle()
    {
        base.PlayCollsionParticle();
        GameSceneManager.Instance.EnemyMagicImpactPool.GetObject<ParticleObject>().PlayOfPosition(transform.position);
    }
}
