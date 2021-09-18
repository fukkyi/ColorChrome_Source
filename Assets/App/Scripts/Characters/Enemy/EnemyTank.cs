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

    protected string meleeAttack2AnimName = "Attack_2";

    protected new void Update()
    {
        base.Update();
        UpdateMoveAnim();
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

        if (!isRangeAttacking)
        {
            // 遠距離攻撃中でない場合はプレイヤーを追う
            agent.SetDestination(detectPlayer.transform.position);
        }
        else
        {
            // 遠距離攻撃中は追従は止めるため、目標点を自分の座標にする
            agent.SetDestination(transform.position);
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
        switch (Random.Range(0, 2))
        {
            case 1:
                AttackToForward(meleeAttackAnimName);
                break;
            case 2:
                AttackToForward(meleeAttack2AnimName);
                break;
        }
    }
}
