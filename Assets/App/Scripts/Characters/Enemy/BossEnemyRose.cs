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

        // ���Ԍo�߂ŉ񕜂�����
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
        // �m����Ԃ����肷��
        isDeadly = (currentHp / (float)maxHp) <= deadlyHpRate;
    }

    /// <summary>
    /// �C���^�[�o�����܂߂���U�����s��
    /// </summary>
    private void SpawnIvyWithInterval()
    {
        if (ivySpawnWaitTime <= 0)
        {
            int attackType = UnityEngine.Random.Range(0, (int)IvyAttackType.TypeCount);
            IvyAttackType ivyAttackType = (IvyAttackType)Enum.ToObject(typeof(IvyAttackType), attackType);
            // �U���^�C�v�ɂ���ďo���ʒu��ς���
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
    /// �C���^�[�o�����܂߂��������U�����s��
    /// </summary>
    private void RangedAttackWithInterval()
    {
        // �����O�ɃG�l���M�[�𒙂߂�p�[�e�B�N�����Đ�������
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
    /// ���@�e�����Ԋu�ŕ����񌂂�
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
    /// �v���C���[�̐^���Ɉ���o��������
    /// </summary>
    /// <param name="attackType"></param>
    private void SpawnIvyOnPlayer(IvyAttackType attackType)
    {
        Vector3 spawnPos = CalcGroundPos(detectPlayer.transform.position);
        SpawnIvy(attackType, spawnPos);
    }

    /// <summary>
    /// �v���C���[�̐^���Ɉ���o��������
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
    /// ����o��������
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
    /// �U�����łȂ�����擾����
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
    /// ����̍��W�ɂ���n�ʂ̍��W���擾����
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
    /// �����_���ȑł��o���ꏊ���疂�@�e������
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
