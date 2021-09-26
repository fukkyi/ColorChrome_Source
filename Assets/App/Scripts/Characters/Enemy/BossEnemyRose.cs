using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEnemyRose : BossEnemy
{
    [SerializeField]
    private RoseIvy[] ivies = null;
    [SerializeField]
    private ParticleSystem energyParticle = null;
    [SerializeField]
    private MagicBulletShooter[] rangedAttackShooters = null;
    [SerializeField]
    private MinMaxRange ivyAroundSpawnRange = new MinMaxRange(0, 10.0f);
    [SerializeField]
    private MinMaxRange ivySpawnInterval = new MinMaxRange(0, 60.0f);
    [SerializeField]
    private MinMaxRange deadlyIvySpawnInterval = new MinMaxRange(0, 60.0f);
    [SerializeField]
    private MinMaxRange rangedAttackInterval = new MinMaxRange(0, 60.0f);

    [SerializeField]
    private float rotateSpeed = 50;
    [SerializeField, Range(0, 1.0f)]
    private float deadlyHpRate = 0.5f;
    [SerializeField]
    private MinMaxRange playEnergyParticleTimeRange = new MinMaxRange(0, 10.0f);
    [SerializeField]
    private float rangedShotInterval = 0.3f;
    [SerializeField]
    private float unDetectedHealInterval = 1.0f;
    [SerializeField]
    private int rangedShotCount = 10;
    [SerializeField]
    private int deadlyRangedShotCount = 20;
    [SerializeField]
    private int healValue = 20;

    private bool isDeadly = false;
    private int currentRangedShotCount = 0;
    private float ivySpawnWaitTime = 0;
    private float rangedAttackWaitTime = 0;
    private float rangedShotWaitTime = 0;
    private float healWaitTime = 0;
    private Vector3 groundRayOffset = Vector3.up * 100;

    protected new void Start()
    {
        ivySpawnWaitTime = ivySpawnInterval.RandOfRange();
        rangedAttackWaitTime = rangedAttackInterval.RandOfRange();

        base.Start();
    }

    protected override void ActionForUndetected()
    {
        base.ActionForUndetected();

        // 時間経過で回復させる
        if (healWaitTime <= 0)
        {
            currentHp = Mathf.Clamp(currentHp += healValue, 0, maxHp);
            healWaitTime = unDetectedHealInterval;
        }
        else
        {
            healWaitTime -= TimeUtil.GetDeltaTime();
        }
    }

    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);

        SpawnIvyWithInterval();
        RangedAttackWithInterval();
    }

    public override void TakeDamage(int attackPower)
    {
        if (!this.enabled) return;

        base.TakeDamage(attackPower);
        // 瀕死状態か判定する
        isDeadly = (currentHp / (float)maxHp) <= deadlyHpRate;
    }

    /// <summary>
    /// インターバルを含めた茨攻撃を行う
    /// </summary>
    private void SpawnIvyWithInterval()
    {
        if (ivySpawnWaitTime <= 0)
        {
            int attackType = UnityEngine.Random.Range(0, (int)IvyAttackType.TypeCount);
            IvyAttackType ivyAttackType = (IvyAttackType)Enum.ToObject(typeof(IvyAttackType), attackType);
            // 攻撃タイプによって出現位置を変える
            if (ivyAttackType == IvyAttackType.Spawn)
            {
                SpawnIvyOnPlayer(ivyAttackType);
            }
            else
            {
                SpawnIvyOnPlayerAround(ivyAttackType);
            }

            float waitTime = isDeadly ? deadlyIvySpawnInterval.RandOfRange() : ivySpawnInterval.RandOfRange();
            ivySpawnWaitTime = waitTime;
        }
        else
        {
            ivySpawnWaitTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// インターバルも含めた遠距離攻撃を行う
    /// </summary>
    private void RangedAttackWithInterval()
    {
        // 撃つ直前にエネルギーを貯めるパーティクルを再生させる
        if (!energyParticle.isPlaying && rangedAttackWaitTime <= playEnergyParticleTimeRange.max)
        {
            energyParticle.Play();
        }
        if (energyParticle.isPlaying && rangedAttackWaitTime <= playEnergyParticleTimeRange.min)
        {
            energyParticle.Stop();
        }

        if (rangedAttackWaitTime <= 0)
        {
            ShotRangedAttackWithInterval();
        }
        else
        {
            rangedAttackWaitTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 魔法弾を一定間隔で複数回撃つ
    /// </summary>
    /// <param name="shotCount"></param>
    private void ShotRangedAttackWithInterval()
    {
        if (rangedShotWaitTime <= 0)
        {
            int shotCount = isDeadly ? deadlyRangedShotCount : rangedShotCount;

            if (currentRangedShotCount < shotCount)
            {
                currentRangedShotCount++;
                ShotRangedAttackByRandomShooter();

                rangedShotWaitTime = rangedShotInterval;
            }
            else
            {
                rangedAttackWaitTime = rangedAttackInterval.RandOfRange();
                rangedShotWaitTime = 0;
                currentRangedShotCount = 0;
            }
        }
        else
        {
            rangedShotWaitTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// プレイヤーの真下に茨を出現させる
    /// </summary>
    /// <param name="attackType"></param>
    private void SpawnIvyOnPlayer(IvyAttackType attackType)
    {
        Vector3 spawnPos = CalcGroundPos(detectPlayer.transform.position);
        SpawnIvy(attackType, spawnPos);
    }

    /// <summary>
    /// プレイヤーの真下に茨を出現させる
    /// </summary>
    /// <param name="attackType"></param>
    private void SpawnIvyOnPlayerAround(IvyAttackType attackType)
    {
        float spawnAngle = UnityEngine.Random.Range(0, 2.0f) * Mathf.PI;
        Vector3 spawnDirection = Vector3.right * Mathf.Sin(spawnAngle) + Vector3.forward * Mathf.Cos(spawnAngle);
        Vector3 spawnPos = detectPlayer.transform.position + spawnDirection * ivyAroundSpawnRange.RandOfRange();

        Vector3 spawnGroundPos = CalcGroundPos(spawnPos + groundRayOffset);

        SpawnIvy(attackType, spawnGroundPos);
    }

    /// <summary>
    /// 茨を出現させる
    /// </summary>
    /// <param name="attackType"></param>
    private void SpawnIvy(IvyAttackType attackType, Vector3 spawnPos)
    {
        RoseIvy ivy = GetUnAttackedIvy();

        if (ivy == null) return;

        switch(attackType)
        {
            case IvyAttackType.Spawn:
                ivy.PlaySpawnAttack(spawnPos);
                break;

            case IvyAttackType.Normal:
                ivy.PlayNormalAttack(spawnPos);
                break;

            case IvyAttackType.Spin:
                ivy.PlaySpinAttack(spawnPos);
                break;
        }
    }

    /// <summary>
    /// 攻撃中でない茨を取得する
    /// </summary>
    /// <returns></returns>
    private RoseIvy GetUnAttackedIvy()
    {
        foreach(RoseIvy ivy in ivies)
        {
            if (ivy.gameObject.activeSelf) continue;

            return ivy;
        }

        return null;
    }

    /// <summary>
    /// 特定の座標にある地面の座標を取得する
    /// </summary>
    /// <param name="clacPos"></param>
    /// <returns></returns>
    private Vector3 CalcGroundPos(Vector3 clacPos)
    {
        RaycastHit raycastHit;
        if (!Physics.Raycast(clacPos, Vector3.down, out raycastHit, Mathf.Infinity, LayerMaskUtil.GetLayerMaskGrounds())) return Vector3.zero;

        return raycastHit.point;
    }

    /// <summary>
    /// ランダムな打ち出し場所から魔法弾を撃つ
    /// </summary>
    protected void ShotRangedAttackByRandomShooter()
    {
        MagicBulletShooter shooter = rangedAttackShooters[UnityEngine.Random.Range(0, rangedAttackShooters.Length)];

        EnemyMagicBullet magicBullet = GameSceneManager.Instance.GetEnemyMagicBulletPool().GetObject<EnemyMagicBullet>();
        shooter.ShotToPosition(magicBullet, detectPlayer.GetCenterPos());

        AudioManager.Instance.PlaySE("Bio gun Shot 10", shooter.transform.position);
    }

    private enum IvyAttackType
    {
        Spawn,
        Normal,
        Spin,
        TypeCount
    }
}
