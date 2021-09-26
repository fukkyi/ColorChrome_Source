using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AgentEnemy : Enemy
{
    [SerializeField]
    protected EnemyAttackCollider meleeAttackCollider = null;
    [SerializeField]
    protected MagicBulletShooter rangedAttackShooter = null;

    [SerializeField]
    protected float rotateSpeed = 5.0f;
    [SerializeField, Range(0, 180)]
    protected float attackAngle = 10;
    [SerializeField]
    protected float strollMoveSpeed = 0.5f;
    [SerializeField]
    protected float chaseMoveSpeed = 2.0f;
    [SerializeField]
    protected float defaultStopDistance = 0.15f;
    [SerializeField]
    protected float chaseStopDistance = 1.5f;
    [SerializeField]
    protected Vector3 strollSize = Vector3.one;

    [SerializeField]
    protected MinMaxRange strollInterval = new MinMaxRange(0, 60.0f);
    [SerializeField]
    protected MinMaxRange rangedAttackInterval = new MinMaxRange(0, 60.0f);

    protected bool isArrivalStroll = false;
    protected bool isPlayingUnableToMoveAnim = false;
    protected float strollWaitTime = 0;
    protected float rangedAttackWaitTime = 0;
    protected string meleeAttackAnimName = "Attack_1";
    protected string rangedAttackAnimName = "Attack_2";

    protected Vector3 strollCenter = Vector3.zero;

    protected NavMeshAgent agent = null;

    // Start is called before the first frame update
    protected new void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        strollCenter = transform.position;
        rangedAttackWaitTime = rangedAttackInterval.RandOfRange();

        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        // �s���s�ɂȂ�A�j���[�V�������Đ����̏ꍇ�͒Ǐ]�n�_�������ɂ���
        if (!isDead && isPlayingUnableToMoveAnim)
        {
            agent.SetDestination(transform.position);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(strollCenter == Vector3.zero ? transform.position : strollCenter, strollSize);
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

    protected override void OnDead()
    {
        base.OnDead();

        agent.enabled = false;
    }

    /// <summary>
    /// ���S�Ɍ��������ۂɍs������
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        agent.speed = strollMoveSpeed;
        agent.stoppingDistance = defaultStopDistance;
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();
        // �ǐՒ��͏�Ɍx����Ԃɂ���
        SetCaution();

        agent.speed = chaseMoveSpeed;
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

    /// <summary>
    /// ���������̍s��
    /// </summary>
    protected override void ActionForUndetected()
    {
        base.ActionForUndetected();

        MoveToStrollArea();
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

    protected override void ActionForFlinching()
    {
        base.ActionForFlinching();

        agent.SetDestination(transform.position);
        OnAttackColliderDisable();
        OnFinishUnableToMoveAnim();
    }

    /// <summary>
    /// �ʏ�U�����s��
    /// </summary>
    protected virtual void NormalAttack()
    {
        AttackToForward(meleeAttackAnimName);
    }

    /// <summary>
    /// �������U�����s��
    /// </summary>
    protected virtual void RangedAttack()
    {
        AttackToForward(rangedAttackAnimName);
    }

    /// <summary>
    /// �C���^�[�o�����܂߂��������U�����s��
    /// </summary>
    protected void RangedAttackWithInterval()
    {
        if (rangedAttackWaitTime <= 0)
        {
            RangedAttack();
            rangedAttackWaitTime = rangedAttackInterval.RandOfRange();
        }
        else if (!isPlayingWaitAnim)
        {
            rangedAttackWaitTime -= Time.deltaTime;
        }
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
    /// �p�j�G���A�������_���Ɉړ�������
    /// </summary>
    protected virtual void MoveToStrollArea()
    {
        if (agent.remainingDistance > agent.stoppingDistance) return;
        // �ڕW�_�ɓ���������ҋ@���Ԃ�ݒ肷��
        if (!isArrivalStroll)
        {
            strollWaitTime = strollInterval.RandOfRange();
            isArrivalStroll = true;
        }
        // �ҋ@���Ԃ��Ȃ��Ȃ�܂őҋ@������
        if (strollWaitTime > 0)
        {
            strollWaitTime = Mathf.Clamp(strollWaitTime - Time.deltaTime, 0, strollInterval.maxLimit);
            return;
        }
        // �������U�����̏ꍇ�͖ڕW�_��ݒ肵�Ȃ�
        if (isPlayingUnableToMoveAnim) return;
        // �ҋ@���Ԃ��I��������V���ȖڕW�_��ݒ肷��
        Vector3 strollPos = TransformUtil.GetRandPosByBox(strollCenter, strollSize);
        agent.SetDestination(strollPos);

        isArrivalStroll = false;
    }

    /// <summary>
    /// �Ђ�܂���
    /// </summary>
    /// <returns></returns>
    public override bool SetFlinch()
    {
        if (!base.SetFlinch()) return false;

        OnAttackColliderDisable();

        return true;
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

    protected void OnShotRangedAttack()
    {
        EnemyMagicBullet magicBullet = GameSceneManager.Instance.GetEnemyMagicBulletPool().GetObject<EnemyMagicBullet>();
        rangedAttackShooter.ShotToPosition(magicBullet, detectPlayer.GetCenterPos());

        AudioManager.Instance.PlaySE("Bio gun Shot 10", rangedAttackShooter.transform.position);
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
    }
}
