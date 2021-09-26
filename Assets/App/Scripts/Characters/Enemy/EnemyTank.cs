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
    /// 発見した直後に行う処理
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();

        animator.SetFloat("WalkAnimSpeed", chaseWalkAnimSpeed);
    }

    /// <summary>
    /// 見失った直後に行う処理
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        animator.SetFloat("WalkAnimSpeed", defaultWalkAnimSpeed);
    }

    /// <summary>
    /// 発見時の行動
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        if (!isPlayingUnableToMoveAnim)
        {
            agent.SetDestination(detectPlayer.transform.position);
        }

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        // 追従目標の停止距離まで近づいているか
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // プレイヤーとの距離が近い場合は近接攻撃
            if (playerDistance <= agent.stoppingDistance)
            {
                NormalAttack();
            }
        }
    }

    /// <summary>
    /// 移動アニメーションの速さなどを更新する
    /// </summary>
    protected void UpdateMoveAnim()
    {
        Vector3 moveVec = agent.velocity;
        moveVec.y = 0;

        animator.SetFloat("MoveSpeed", moveVec.magnitude);
    }

    /// <summary>
    /// 通常攻撃を行う
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
    /// ひるませる
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
    /// AnimationEventから受け取ったタイミングで攻撃判定を有効にする
    /// </summary>
    private void OnAttack2ColliderEneble()
    {
        if (IsFlinching()) return;

        meleeAttack2Collider.EnableCollider();
    }

    /// <summary>
    /// AnimationEventから受け取ったタイミングで攻撃判定を無効にする
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
