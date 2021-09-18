using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicBullet : MagicBullet
{
    /// <summary>
    /// è’ìÀÇµÇΩç€ÇÃèàóù
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionAction(Collision collision)
    {
        base.OnCollisionAction(collision);

        AudioManager.Instance.PlaySE("Buff 1", transform.position);

        if (collision.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        Player playerComponent = collision.gameObject.GetComponent<Player>();

        // ä˘Ç…É_ÉÅÅ[ÉWÇó^Ç¶ÇΩÇ‡ÇÃÇ»ÇÁñ≥éãÇ∑ÇÈ
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
