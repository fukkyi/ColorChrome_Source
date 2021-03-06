using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicBullet : MagicBullet
{
    /// <summary>
    /// 衝突した際の処理
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionAction(Collision collision)
    {
        base.OnCollisionAction(collision);

        AudioManager.Instance.PlaySE("Debuff 19", transform.position);

        if (collision.gameObject.layer != LayerMaskUtil.EnemyLayerNumber) return;

        Enemy enemyComponent = collision.gameObject.GetComponentInParent<Enemy>();

        // 既にダメージを与えたものなら無視する
        if (damagedObjectList.Contains(enemyComponent.gameObject)) return;

        enemyComponent.TakeDamage(attackPower);
        damagedObjectList.Add(enemyComponent.gameObject);
    }

    protected override void PlayCollsionParticle()
    {
        base.PlayCollsionParticle();
        GameSceneManager.Instance.PlayerMagicImpactPool.GetObject<ParticleObject>().PlayOfPosition(transform.position);
    }
}
