using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicBullet : MagicBullet
{
    /// <summary>
    /// è’ìÀÇµÇΩç€ÇÃèàóù
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionAction(Collision collision)
    {
        base.OnCollisionAction(collision);

        AudioManager.Instance.PlaySE("Debuff 19", transform.position);

        if (collision.gameObject.layer != LayerMaskUtil.EnemyLayerNumber) return;

        Enemy enemyComponent = collision.gameObject.GetComponent<Enemy>();

        // ä˘Ç…É_ÉÅÅ[ÉWÇó^Ç¶ÇΩÇ‡ÇÃÇ»ÇÁñ≥éãÇ∑ÇÈ
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
