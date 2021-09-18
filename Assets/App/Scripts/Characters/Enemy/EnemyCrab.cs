using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyCrab : AgentEnemy
{
    [SerializeField]
    private float bodySize = 1.0f;
    [SerializeField]
    private float approachDistance = 4.0f;
    [SerializeField]
    private float playerSeparateDistance = 8.0f;
    [SerializeField]
    private float heightChangeSpeed = 10.0f;
    [SerializeField]
    MinMaxRange flyHeightLimit = new MinMaxRange(0, 50);

    private Vector3 targetPos = Vector3.zero;

    private float baseFlyHeight = 0;
    private float currentFlyHeight = 0;
    private float targetHeightPadding = 0.1f;

    protected new void Start()
    {
        base.Start();

        currentFlyHeight = agent.baseOffset;
        baseFlyHeight = agent.baseOffset;
    }

    /// <summary>
    /// �|���ꂽ�ۂ̏���
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();

        myRb.useGravity = true;
    }

    protected override void OnDetected()
    {
        base.OnDetected();
    }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        SetUpdateRotation(true);
    }

    /// <summary>
    /// ���������̍s��
    /// </summary>
    protected override void ActionForUndetected()
    {
        base.ActionForUndetected();

        ChangeFlyHeightToBase();
    }

    /// <summary>
    /// �������̍s��
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        float playerDistance2D = TransformUtil.Calc2DDistance(transform.position, detectPlayer.transform.position);
        // �Ǐ]�ڕW�̒�~�����܂ŋ߂Â��Ă��邩
        if (playerDistance <= chaseStopDistance)
        {
            SetUpdateRotation(false);
            NormalAttack();
        }
        // �v���C���[�Ƃ̋������߂��ꍇ�̓v���C���[�ɋ߂Â�
        else if (playerDistance2D <= approachDistance) 
        {
            SetUpdateRotation(true);
            // �d������A�j���[�V�������Đ����Ă��Ȃ��ꍇ�͒Ǐ]�ڕW�n�_���v���C���[�ɐݒ肷��
            if (!isPlayingWaitAnim)
            {
                agent.SetDestination(detectPlayer.transform.position);
                // �v���C���[�ɔ�s���鍂�������킹��
                if (ChangeFlyHeightToPlayer())
                {
                    // ���������킹��ꂸ�A�v���C���[�̋���������ꍇ�͉������U�����s��
                    RangedAttackWithInterval();
                }
            }
        }
        // �v���C���[�Ƃ̋���������Ă���ꍇ�́A�v���C���[�ƈ��̋�����ۂ悤�Ɉړ�����
        else
        {
            Vector3 playerDirection2D = TransformUtil.Calc2DDirection(transform.position, detectPlayer.transform.position);
            Vector3 separatedPos = -playerDirection2D * (playerSeparateDistance - playerDistance2D);

            SetUpdateRotation(false);
            ChangeFlyHeightToBase();

            agent.SetDestination(transform.position + separatedPos);
            // ��Ƀv���C���[�Ɍ����悤�ɉ�]����
            transform.rotation = RotateTowardsToDetectPlayer(rotateSpeed);

            if (IsTherePlayerByforward(attackAngle))
            {
                RangedAttackWithInterval();
            }
        }
    }

    private void SetTargetPosForStrollPos()
    {
        Vector3 myPos = transform.position;
        // �ҋ@���Ԃ��I��������V���ȖڕW�_��ݒ肷��
        targetPos = TransformUtil.GetRandPosByBox(strollCenter, strollSize);

        Vector3 targetDirection = targetPos - myPos;
        float targetDisrance = Vector3.Distance(targetPos, myPos);
        // �̂̑傫����SphereRay�𔭎˂���
        if (Physics.SphereCast(myPos, bodySize, targetDirection, out RaycastHit raycastHit, targetDisrance, LayerMaskUtil.GetLayerMaskGrounds()))
        {
            // ��Q�������m������̏�Q���̎�O��ڕW�̍��W�ɂ���
            targetPos = myPos + (targetDirection.normalized * raycastHit.distance);
        }
    }

    /// <summary>
    /// NavMeshAgent�ɉ�]��C���邩�ݒ肷��
    /// </summary>
    /// <param name="enable"></param>
    private void SetUpdateRotation(bool enable)
    {
        if (agent.updateRotation == enable) return;

        agent.updateRotation = enable;
    }

    /// <summary>
    /// �v���C���[�ɒǂ��悤�ɔ�s���鍂����ς���
    /// ��s�̍�����ς��Ȃ��ꍇ��Ture��Ԃ�
    /// </summary>
    /// <returns></returns>
    private bool ChangeFlyHeightToPlayer()
    {
        if (detectPlayer == null) return true;

        float heightDiff = transform.position.y - detectPlayer.transform.position.y;

        return ChangeFlyHeight(heightDiff, heightChangeSpeed);
    }

    /// <summary>
    /// ���̔�s���鍂���ɖ߂�悤�ɍ�����ς���
    /// </summary>
    private void ChangeFlyHeightToBase()
    {
        float heightDiff = currentFlyHeight - baseFlyHeight;

        if (Mathf.Abs(heightDiff) <= targetHeightPadding) return;

        ChangeFlyHeight(heightDiff, heightChangeSpeed);
    }

    /// <summary>
    /// ��s���鍂����ς���
    /// </summary>
    /// <param name="heightDiff"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private bool ChangeFlyHeight(float heightDiff, float speed)
    {
        if (Mathf.Abs(heightDiff) <= targetHeightPadding) return true;

        float changeHeightValue = speed * TimeUtil.GetDeltaTime();

        if (heightDiff > 0)
        {
            // �ڕW�̍�����荂���ꍇ
            if (currentFlyHeight == flyHeightLimit.min) return true;
            currentFlyHeight -= changeHeightValue;
        }
        else
        {
            // �����̍������Ⴂ�ꍇ
            if (currentFlyHeight == flyHeightLimit.max) return true;
            currentFlyHeight += changeHeightValue;
        }

        currentFlyHeight = Mathf.Clamp(currentFlyHeight, flyHeightLimit.min, flyHeightLimit.max);
        agent.baseOffset = currentFlyHeight;

        return false;
    }
}
