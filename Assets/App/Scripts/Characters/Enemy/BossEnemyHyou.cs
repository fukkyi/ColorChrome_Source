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
        // 行動不可になるアニメーションが再生中の場合は追従地点を自分にする
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
    /// 完全に見失った際に行う処理
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        agent.stoppingDistance = defaultStopDistance;
        agent.SetDestination(startPos);
    }

    /// <summary>
    /// 発見した直後に行う処理
    /// </summary>
    protected override void OnDetected()
    {
        base.OnDetected();
        // 追跡中は常に警戒状態にする
        SetCaution();

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

    protected override void OnDead()
    {
        base.OnDead();

        agent.enabled = false;
    }

    /// <summary>
    /// 発見時の行動
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        if (isPlayingUnableToMoveAnim) return;
        // 攻撃インターバル中はここで処理を止める
        if (!UpdateBreakingInterval()) return;
        // 遠距離攻撃を行う場合はここで処理を止める
        if (UpdateRangedAttackInterval()) return;

        UpdateSpecialAttackInterval();

        // 遠距離攻撃中でない場合はプレイヤーを追う
        agent.SetDestination(detectPlayer.transform.position);

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        // 追従目標の停止距離まで近づいているか
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (isReadySpecialAttack)
            {
                // 大攻撃の準備が整っている場合は大攻撃を行う
                SpecialAttack();
            }
            else if (playerDistance <= meleeDistance)
            {
                // プレイヤーとの距離が近い場合は近接攻撃
                MeleeAttack();
            }
            else
            {
                // プレイヤーとの距離が遠い場合は遠距離攻撃
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
    /// 見失った時の行動
    /// </summary>
    protected override void ActionForLoseSight()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeEnemySearchStatus(EnemySearchStatus.undetected);
        }
    }

    /// <summary>
    /// 通常近接攻撃を行う
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
    /// 遠距離攻撃を行う
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
    /// 大攻撃を行う
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
    /// ひるませる
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
    /// 移動アニメーションの速さなどを更新する
    /// </summary>
    protected void UpdateMoveAnim()
    {
        Vector3 moveVec = agent.velocity;
        moveVec.y = 0;

        animator.SetFloat("MoveSpeed", moveVec.magnitude);
    }

    /// <summary>
    /// 大攻撃の準備時間を更新する
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
    /// 攻撃インターバルの更新する
    /// </summary>
    /// <returns></returns>
    protected bool UpdateBreakingInterval()
    {
        if (breakingTime <= 0) return true;
        // 休憩中はその場でプレイヤーの方を向き続ける
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
    /// 遠距離攻撃のインターバルを更新する
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
    /// アニメーション用のトリガーをリセットする
    /// </summary>
    private void ResetAnimTrigger()
    {
        animator.ResetTrigger(meleeAttackAnimName);
        animator.ResetTrigger(rangedAttackAnimName);
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

    /// <summary>
    /// AnimationEventから受け取ったタイミングで攻撃判定を有効にする
    /// </summary>
    private void OnSpecialAttackColliderEnable()
    {
        if (IsFlinching()) return;

        specialAttackCollider.EnableCollider();
        specialAttackExplodeObject.StartExplode();
        specialExplosionParticle.Play();
    }

    /// <summary>
    /// AnimationEventから受け取ったタイミングで攻撃判定を無効にする
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
    /// AnimationEventから受け取ったタイミングで大攻撃の魔法弾を撃つ
    /// </summary>
    private void OnShotSpecialAttack()
    {
        Vector3 bulletDirection = Vector3.zero;
        int shotCount = currentHp > deadlyHp ? specialShotCount : deadlySpecialShotCount;
        float shotIntervalAngle = specialShotAngleRange / shotCount;
        // 全方位(n)wayに魔法弾を撃つ
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
        ResetAnimTrigger();
    }
}
