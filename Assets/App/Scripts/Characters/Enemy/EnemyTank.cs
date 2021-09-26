using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : AgentEnemy
{
    [SerializeField]
    private EnemyAttackCollider meleeAttack2Collider = null;
    [SerializeField]
    private float defaultWalkAnimSpeed = 1.0f;
    [SerializeField]
    private float chaseWalkAnimSpeed = 2.0f;
    [SerializeField]
    private int meleeAttack1Power = 20;
    [SerializeField]
    private int meleeAttack2Power = 25;

    protected string meleeAttack2AnimName = "Attack_2";

    protected new void Update()
    {
        base.Update();
        UpdateMoveAnim();
    }

    public override void TakeDamage(int attackPower)
    {
        base.TakeDamage(attackPower);
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

        if (!isPlayingUnableToMoveAnim)
        {
            agent.SetDestination(detectPlayer.transform.position);
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
        }
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
    /// �ʏ�U�����s��
    /// </summary>
    protected override void NormalAttack()
    {
        switch (Random.Range(0, (int)MeleeAttackType.AttackCount))
        {
            case (int)MeleeAttackType.Attack1:
                AttackToForward(meleeAttackAnimName);
                break;
            case (int)MeleeAttackType.Attack2:
                AttackToForward(meleeAttack2AnimName);
                break;
        }
    }

    /// <summary>
    /// �Ђ�܂���
    /// </summary>
    /// <returns></returns>
    public override bool SetFlinch()
    {
        if (!base.SetFlinch()) return false;

        OnAttackColliderDisable();
        OnAttack2ColliderDisable();
        currentFlinchTime = flinchTime;

        return true;
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU�������L���ɂ���
    /// </summary>
    private void OnAttack2ColliderEneble()
    {
        if (IsFlinching()) return;

        meleeAttack2Collider.EnableCollider();
    }

    /// <summary>
    /// AnimationEvent����󂯎�����^�C�~���O�ōU������𖳌��ɂ���
    /// </summary>
    private void OnAttack2ColliderDisable()
    {
        meleeAttack2Collider.DisableCollider();
    }

    private void OnFootsteps()
    {
        AudioManager.Instance.PlayRandomPitchSE("Huge monster footsteps 1", transform.position);
    }

    private enum MeleeAttackType
    {
        Attack1,
        Attack2,
        AttackCount
    }
}
