using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroom : AgentEnemy
{
    [SerializeField]
    private float rangedShotDistance = 8.0f;
    [SerializeField]
    private float defaultWalkAnimSpeed = 1.0f;
    [SerializeField]
    private float chaseWalkAnimSpeed = 2.0f;

    protected string meleeAttack2AnimName = "Attack_2";

    protected new void Update()
    {
        base.Update();
        UpdateMoveAnim();
    }

    /// <summary>
    /// �|���ꂽ�ۂ̏���
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();

        MissionManager.Instance.AddCountOfMission(MissionName.MushEnemyKill, 1);
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        animator.SetFloat("WalkAnimSpeed", defaultWalkAnimSpeed);
    }

    /// <summary>
    /// �������̍s��
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        if (!isRangeAttacking)
        {
            // �������U�����łȂ��ꍇ�̓v���C���[��ǂ�
            agent.SetDestination(detectPlayer.transform.position);
        }
        else
        {
            // �������U�����͒Ǐ]�͎~�߂邽�߁A�ڕW�_�������̍��W�ɂ���
            agent.SetDestination(transform.position);
        }

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        // �Ǐ]�ڕW�̒�~�����܂ŋ߂Â��Ă��邩
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // �v���C���[�Ƃ̋������߂��ꍇ�͋ߐڍU��
            if (playerDistance <= agent.stoppingDistance)
            {
                NormalAttack();
            }
            // �v���C���[�Ƃ̋����������ꍇ�͉������U��
            else
            {
                RangedAttack();
            }
        }
        // �������U���͈̔͂ɂ���ꍇ
        else if (playerDistance >= rangedShotDistance)
        {
            // �C���^�[�o�����܂߂ĉ������U�����s��
            RangedAttackWithInterval();
        }
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();

        animator.SetFloat("WalkAnimSpeed", chaseWalkAnimSpeed);
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
}
