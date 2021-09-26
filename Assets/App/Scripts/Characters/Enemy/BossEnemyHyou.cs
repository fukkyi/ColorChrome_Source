using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BossEnemyHyou : BossEnemy
{
    [SerializeField]
    private EnemyAttackCollider meleeAttackCollider = null;
    [SerializeField]
    private EnemyAttackCollider specialAttackCollider = null;
    [SerializeField]
    private MagicBulletShooter rangedAttackShooter = null;
    [SerializeField]
    private MagicBulletShooter specialAttackShooter = null;
    [SerializeField]
    private ParticleSystem specialExplosionParticle = null;
    [SerializeField]
    private ExplodeObject specialAttackExplodeObject = null;
    [SerializeField]
    private MinMaxRange rangedAttackInterval = new MinMaxRange(0, 60.0f);
    [SerializeField]
    private MinMaxRange specialAttackInterval = new MinMaxRange(0, 60.0f);
    [SerializeField]
    private MinMaxRange normalAttackBreakingInterval = new MinMaxRange(0, 10.0f);
    [SerializeField]
    private MinMaxRange specialAttackBreakingInterval = new MinMaxRange(0, 10.0f);
    [SerializeField]
    private float rotateSpeed = 5.0f;
    [SerializeField, Range(0, 180)]
    private float attackAngle = 10;
    [SerializeField]
    private float defaultStopDistance = 0.15f;
    [SerializeField]
    private float chaseStopDistance = 1.5f;
    [SerializeField]
    private float meleeDistance = 1.0f;
    [SerializeField]
    private int specialShotCount = 8;
    [SerializeField]
    private int deadlySpecialShotCount = 12;
    [SerializeField]
    private int deadlyHp = 400;

    private Vector3 startPos = Vector3.zero;
    private NavMeshAgent agent = null;

    private bool isPlayingUnableToMoveAnim = false;
    private bool isReadySpecialAttack = false;
    private bool isSpecialAttacking = false;
    private float rangedAttackWaitTime = 0;
    private float specialAttackWaitTime = 0;
    private float specialShotAngleRange = 360;
    private float breakingTime = 0;
    private string meleeAttackAnimName = "Attack_1";
    private string rangedAttackAnimName = "Attack_2";
    private string specialAttackAnimName = "Attack_3";

    protected new void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        specialAttackWaitTime = specialAttackInterval.RandOfRange();

        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        UpdateMoveAnim();
        // �s���s�ɂȂ�A�j���[�V�������Đ����̏ꍇ�͒Ǐ]�n�_�������ɂ���
        if (!isDead && isPlayingUnableToMoveAnim)
        {
            agent.SetDestination(transform.position);
        }
    }

    protected new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (agent == null) return;

        Gizmos.color = Color.red;
        Vector3 pathPos = transform.position;
        foreach (var pos in agent.path.corners)
        {
            Gizmos.DrawLine(pathPos, pos);
            pathPos = pos;
        }

        Gizmos.DrawSphere(agent.pathEndPosition, 0.5f);
    }

    /// <summary>
    /// ���S�Ɍ��������ۂɍs������
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        agent.stoppingDistance = defaultStopDistance;
        agent.SetDestination(startPos);
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();
        // �ǐՒ��͏�Ɍx����Ԃɂ���
        SetCaution();

        agent.stoppingDistance = chaseStopDistance;
        // �������U�����͒Ǐ]�ڕW��ݒ肵�Ȃ�
        if (isPlayingUnableToMoveAnim) return;

        agent.SetDestination(detectPlayer.transform.position);
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnLoseSight()
    {
        base.OnLoseSight();

        agent.stoppingDistance = defaultStopDistance;
    }

    protected override void OnDead()
    {
        base.OnDead();

        agent.enabled = false;
    }

    /// <summary>
    /// �������̍s��
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        if (isPlayingUnableToMoveAnim) return;
        // �U���C���^�[�o�����͂����ŏ������~�߂�
        if (!UpdateBreakingInterval()) return;
        // �������U�����s���ꍇ�͂����ŏ������~�߂�
        if (UpdateRangedAttackInterval()) return;

        UpdateSpecialAttackInterval();

        // �������U�����łȂ��ꍇ�̓v���C���[��ǂ�
        agent.SetDestination(detectPlayer.transform.position);

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        // �Ǐ]�ڕW�̒�~�����܂ŋ߂Â��Ă��邩
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (isReadySpecialAttack)
            {
                // ��U���̏����������Ă���ꍇ�͑�U�����s��
                SpecialAttack();
            }
            else if (playerDistance <= meleeDistance)
            {
                // �v���C���[�Ƃ̋������߂��ꍇ�͋ߐڍU��
                MeleeAttack();
            }
            else
            {
                // �v���C���[�Ƃ̋����������ꍇ�͉������U��
                RangedAttack();
            }
        }
    }

    protected override void ActionForFlinching()
    {
        base.ActionForFlinching();

        agent.SetDestination(transform.position);
        OnAttackColliderDisable();
        OnFinishUnableToMoveAnim();
    }

    /// <summary>
    /// �����������̍s��
    /// </summary>
    protected override void ActionForLoseSight()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeEnemySearchStatus(EnemySearchStatus.undetected);
        }
    }

    /// <summary>
    /// �ʏ�ߐڍU�����s��
    /// </summary>
    protected virtual void MeleeAttack()
    {
        AttackToForward(meleeAttackAnimName,
            onFinishAttackAction: () => {
                breakingTime = normalAttackBreakingInterval.RandOfRange();
            }
        );
    }

    /// <summary>
    /// �������U�����s��
    /// </summary>
    protected virtual void RangedAttack()
    {
        AttackToForward(rangedAttackAnimName,
            () => {
                rangedAttackWaitTime = rangedAttackInterval.RandOfRange();
            },
            () => {
                breakingTime = normalAttackBreakingInterval.RandOfRange();
            }
        );
    }

    /// <summary>
    /// ��U�����s��
    /// </summary>
    protected virtual void SpecialAttack()
    {
        AttackToForward(specialAttackAnimName,
            () => { 
                isSpecialAttacking = true;
            },
            () => { 
                isSpecialAttacking = false;
                isReadySpecialAttack = false;
                breakingTime = specialAttackBreakingInterval.RandOfRange();
            }
        );
    }

    /// <summary>
    /// �v���C���[�𐳖ʂɌ����Ă̍U�����s��
    /// </summary>
    /// <param name="animName"></param>
    protected void AttackToForward(string animName, Action onStartAttackAction = null, Action onFinishAttackAction = null)
    {
        if (isPlayingWaitAnim) return;

        if (IsTherePlayerByforward(attackAngle))
        {
            animator.SetTrigger(animName);
            onStartAttackAction?.Invoke();

            StartCoroutine(WatchPlayeringAnimState(animName, onPlayedAction: onFinishAttackAction));
        }

        transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);
    }

    /// <summary>
    /// �Ђ�܂���
    /// </summary>
    /// <returns></returns>
    public override bool SetFlinch()
    {
        if (!base.SetFlinch()) return false;
        if (isSpecialAttacking) return false;

        OnAttackColliderDisable();
        OnSpecialAttackColliderDisable();

        return true;
    }

    /// <summary>
    /// �ړ��A�j���[�V�����̑����Ȃǂ��X�V����
    /// </summary>
    protected void UpdateMoveAnim()
    {
        Vector3 moveVec = agent.velocity;
        moveVec.y = 0;

        animator.SetFloat("MoveSpeed", moveVec.magnitude);
    }

    /// <summary>
    /// ��U���̏������Ԃ��X�V����
    /// </summary>
    protected void UpdateSpecialAttackInterval()
    {
        if (isReadySpecialAttack) return;

        if (specialAttackWaitTime <= 0)
        {
            specialAttackWaitTime = specialAttackInterval.RandOfRange();
            isReadySpecialAttack = true;
        }
        else
        {
            specialAttackWaitTime -= TimeUtil.GetDeltaTime();
        }
    }

    /// <summary>
    /// �U���C���^�[�o���̍X�V����
    /// </summary>
    /// <returns></returns>
    protected bool UpdateBreakingInterval()
    {
        if (breakingTime <= 0) return true;
        // �x�e���͂��̏�Ńv���C���[�̕�������������
        agent.SetDestination(transform.position);
        transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);

        breakingTime -= TimeUtil.GetDeltaTime();

        if (breakingTime <= 0)
        {
            agent.SetDestination(detectPlayer.transform.position);
        }

        return false;
    }

    /// <summary>
    /// �������U���̃C���^�[�o�����X�V����
    /// </summary>
    /// <returns></returns>
    protected bool UpdateRangedAttackInterval()
    {
        rangedAttackWaitTime -= TimeUtil.GetDeltaTime();

        if (rangedAttackWaitTime > 1.0f) return false;

        agent.SetDestination(transform.position);

        if (rangedAttackWaitTime <= 0)
        {
            RangedAttack();
        }
        else
        {
            transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);
        }

        return true;
    }

    /// <summary>
    /// �A�j���[�V�����p�̃g���K�[�����Z�b�g����
    /// </summary>
    private void ResetAnimTrigger()
    {
        animator.ResetTrigger(meleeAttackAnimName);
        animator.ResetTrigger(rangedAttackAnimName);
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU�������L���ɂ���
    /// </summary>
    protected void OnAttackColliderEneble()
    {
        if (IsFlinching()) return;

        meleeAttackCollider.EnableCollider();
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU������𖳌��ɂ���
    /// </summary>
    protected void OnAttackColliderDisable()
    {
        meleeAttackCollider.DisableCollider();
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU�������L���ɂ���
    /// </summary>
    private void OnSpecialAttackColliderEnable()
    {
        if (IsFlinching()) return;

        specialAttackCollider.EnableCollider();
        specialAttackExplodeObject.StartExplode();
        specialExplosionParticle.Play();
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU������𖳌��ɂ���
    /// </summary>
    private void OnSpecialAttackColliderDisable()
    {
        specialAttackCollider.DisableCollider();
    }

    protected void OnShotRangedAttack()
    {
        EnemyMagicBullet magicBullet = GameSceneManager.Instance.GetEnemyMagicBulletPool().GetObject<EnemyMagicBullet>();
        rangedAttackShooter.ShotToPosition(magicBullet, detectPlayer.GetCenterPos());

        AudioManager.Instance.PlayRandomPitchSE("Bio gun Shot 10", rangedAttackShooter.transform.position);
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ő�U���̖��@�e������
    /// </summary>
    private void OnShotSpecialAttack()
    {
        Vector3 bulletDirection = Vector3.zero;
        int shotCount = currentHp > deadlyHp ? specialShotCount : deadlySpecialShotCount;
        float shotIntervalAngle = specialShotAngleRange / shotCount;
        // �S����(n)way�ɖ��@�e������
        for(int i = 0; i < shotCount; i++)
        {
            EnemyMagicBullet magicBullet = GameSceneManager.Instance.GetEnemyMagicBulletPool().GetObject<EnemyMagicBullet>();

            float shotAngle = shotIntervalAngle * i;

            bulletDirection.x = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
            bulletDirection.z = Mathf.Sin(shotAngle * Mathf.Deg2Rad);

            rangedAttackShooter.ShotToDirection(magicBullet, bulletDirection);
        }

        AudioManager.Instance.PlaySE("Magical Impact 3", specialAttackShooter.transform.position);
    }

    /// <summary>
    /// �s���s�\�ɂȂ�A�j���[�V�������Đ����ꂽ�ۂ̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnStartUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = true;
    }

    /// <summary>
    /// �s���s�\�ɂȂ�A�j���[�V�������I�������ۂ̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnFinishUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = false;
        ResetAnimTrigger();
    }
}
