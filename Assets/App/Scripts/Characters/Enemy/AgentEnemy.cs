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
        // 行動不可になるアニメーションが再生中の場合は追従地点を自分にする
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
    /// 完全に見失った際に行う処理
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        agent.speed = strollMoveSpeed;
        agent.stoppingDistance = defaultStopDistance;
    }

    /// <summary>
    /// 発見した直後に行う処理
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();
        // 追跡中は常に警戒状態にする
        SetCaution();

        agent.speed = chaseMoveSpeed;
        agent.stoppingDistance = chaseStopDistance;
        // 遠距離攻撃中は追従目標を設定しない
        if (isPlayingUnableToMoveAnim) return;

        agent.SetDestination(detectPlayer.transform.position);
    }

    /// <summary>
    /// 見失った直後に行う処理
    /// </summary>
    protected override void OnLoseSight()
    {
        base.OnLoseSight();

        agent.stoppingDistance = defaultStopDistance;
    }

    /// <summary>
    /// 未発見時の行動
    /// </summary>
    protected override void ActionForUndetected()
    {
        base.ActionForUndetected();

        MoveToStrollArea();
    }

    /// <summary>
    /// 見失った時の行動
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
    /// 通常攻撃を行う
    /// </summary>
    protected virtual void NormalAttack()
    {
        AttackToForward(meleeAttackAnimName);
    }

    /// <summary>
    /// 遠距離攻撃を行う
    /// </summary>
    protected virtual void RangedAttack()
    {
        AttackToForward(rangedAttackAnimName);
    }

    /// <summary>
    /// インターバルも含めた遠距離攻撃を行う
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
    /// プレイヤーを正面に向けての攻撃を行う
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
    /// 徘徊エリアをランダムに移動させる
    /// </summary>
    protected virtual void MoveToStrollArea()
    {
        if (agent.remainingDistance > agent.stoppingDistance) return;
        // 目標点に到着したら待機時間を設定する
        if (!isArrivalStroll)
        {
            strollWaitTime = strollInterval.RandOfRange();
            isArrivalStroll = true;
        }
        // 待機時間がなくなるまで待機させる
        if (strollWaitTime > 0)
        {
            strollWaitTime = Mathf.Clamp(strollWaitTime - Time.deltaTime, 0, strollInterval.maxLimit);
            return;
        }
        // 遠距離攻撃中の場合は目標点を設定しない
        if (isPlayingUnableToMoveAnim) return;
        // 待機時間が終了したら新たな目標点を設定する
        Vector3 strollPos = TransformUtil.GetRandPosByBox(strollCenter, strollSize);
        agent.SetDestination(strollPos);

        isArrivalStroll = false;
    }

    /// <summary>
    /// ひるませる
    /// </summary>
    /// <returns></returns>
    public override bool SetFlinch()
    {
        if (!base.SetFlinch()) return false;

        OnAttackColliderDisable();

        return true;
    }

    /// <summary>
    /// AnimationEventから受け取ったタイミングで攻撃判定を有効にする
    /// </summary>
    protected void OnAttackColliderEneble()
    {
        if (IsFlinching()) return;

        meleeAttackCollider.EnableCollider();
    }

    /// <summary>
    /// AnimationEventから受け取ったタイミングで攻撃判定を無効にする
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
    /// 行動不能になるアニメーションが再生された際のイベント (アニメーションイベント用)
    /// </summary>
    private void OnStartUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = true;
    }

    /// <summary>
    /// 行動不能になるアニメーションが終了した際のイベント (アニメーションイベント用)
    /// </summary>
    private void OnFinishUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = false;
    }
}
