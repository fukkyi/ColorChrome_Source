using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBulletShooter : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private int attackPower = 10;
    [SerializeField]
    private float lifeTime = 1.0f;

    /// <summary>
    /// “Á’è•ûŒü‚É–‚–@’e‚ğŒ‚‚Â
    /// </summary>
    /// <param name="magicBullet"></param>
    /// <param name="direction"></param>
    public void ShotToDirection(MagicBullet magicBullet, Vector3 direction)
    {
        magicBullet.Shot(transform.position, direction * speed, attackPower, lifeTime);
    }

    /// <summary>
    /// À•W‚ÉŒü‚¯‚Ä–‚–@’e‚ğŒ‚‚Â
    /// </summary>
    /// <param name="magicBullet"></param>
    /// <param name="shotPosition"></param>
    public void ShotToPosition(MagicBullet magicBullet, Vector3 shotPosition)
    {
        Vector3 direction = (shotPosition - transform.position).normalized;
        ShotToDirection(magicBullet, direction);
    }
}
