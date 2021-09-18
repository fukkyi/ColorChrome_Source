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
    /// 倒された際の処理
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();

        MissionManager.Instance.AddCountOfMission(MissionName.MushEnemyKill, 1);
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
            // プレイヤーとの距離が遠い場合は遠距離攻撃
            else
            {
                RangedAttack();
            }
        }
        // 遠距離攻撃の範囲にいる場合
        else if (playerDistance >= rangedShotDistance)
        {
            // インターバルを含めて遠距離攻撃を行う
            RangedAttackWithInterval();
        }
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
    /// 移動アニメーションの速さなどを更新する
    /// </summary>
    protected void UpdateMoveAnim()
    {
        Vector3 moveVec = agent.velocity;
        moveVec.y = 0;

        animator.SetFloat("MoveSpeed", moveVec.magnitude);
    }
}
