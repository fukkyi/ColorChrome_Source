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
    /// 倒された際の処理
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
    /// 見失った直後に行う処理
    /// </summary>
    protected override void OnUndetected()
    {
        base.OnUndetected();

        SetUpdateRotation(true);
    }

    /// <summary>
    /// 未発見時の行動
    /// </summary>
    protected override void ActionForUndetected()
    {
        base.ActionForUndetected();

        ChangeFlyHeightToBase();
    }

    /// <summary>
    /// 発見時の行動
    /// </summary>
    protected override void ActionForDetected()
    {
        base.ActionForDetected();

        float playerDistance = Vector3.Distance(transform.position, detectPlayer.transform.position);
        float playerDistance2D = TransformUtil.Calc2DDistance(transform.position, detectPlayer.transform.position);
        // 追従目標の停止距離まで近づいているか
        if (playerDistance <= chaseStopDistance)
        {
            SetUpdateRotation(false);
            NormalAttack();
        }
        // プレイヤーとの距離が近い場合はプレイヤーに近づく
        else if (playerDistance2D <= approachDistance) 
        {
            SetUpdateRotation(true);
            // 硬直するアニメーションを再生していない場合は追従目標地点をプレイヤーに設定する
            if (!isPlayingWaitAnim)
            {
                agent.SetDestination(detectPlayer.transform.position);
                // プレイヤーに飛行する高さを合わせる
                if (ChangeFlyHeightToPlayer())
                {
                    // 高さを合わせられず、プレイヤーの距離がある場合は遠距離攻撃を行う
                    RangedAttackWithInterval();
                }
            }
        }
        // プレイヤーとの距離が離れている場合は、プレイヤーと一定の距離を保つように移動する
        else
        {
            Vector3 playerDirection2D = TransformUtil.Calc2DDirection(transform.position, detectPlayer.transform.position);
            Vector3 separatedPos = -playerDirection2D * (playerSeparateDistance - playerDistance2D);

            SetUpdateRotation(false);
            ChangeFlyHeightToBase();

            agent.SetDestination(transform.position + separatedPos);
            // 常にプレイヤーに向くように回転する
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
        // 待機時間が終了したら新たな目標点を設定する
        targetPos = TransformUtil.GetRandPosByBox(strollCenter, strollSize);

        Vector3 targetDirection = targetPos - myPos;
        float targetDisrance = Vector3.Distance(targetPos, myPos);
        // 体の大きさのSphereRayを発射する
        if (Physics.SphereCast(myPos, bodySize, targetDirection, out RaycastHit raycastHit, targetDisrance, LayerMaskUtil.GetLayerMaskGrounds()))
        {
            // 障害物を検知したらの障害物の手前を目標の座標にする
            targetPos = myPos + (targetDirection.normalized * raycastHit.distance);
        }
    }

    /// <summary>
    /// NavMeshAgentに回転を任せるか設定する
    /// </summary>
    /// <param name="enable"></param>
    private void SetUpdateRotation(bool enable)
    {
        if (agent.updateRotation == enable) return;

        agent.updateRotation = enable;
    }

    /// <summary>
    /// プレイヤーに追うように飛行する高さを変える
    /// 飛行の高さを変えない場合はTureを返す
    /// </summary>
    /// <returns></returns>
    private bool ChangeFlyHeightToPlayer()
    {
        if (detectPlayer == null) return true;

        float heightDiff = transform.position.y - detectPlayer.transform.position.y;

        return ChangeFlyHeight(heightDiff, heightChangeSpeed);
    }

    /// <summary>
    /// 元の飛行する高さに戻るように高さを変える
    /// </summary>
    private void ChangeFlyHeightToBase()
    {
        float heightDiff = currentFlyHeight - baseFlyHeight;

        if (Mathf.Abs(heightDiff) <= targetHeightPadding) return;

        ChangeFlyHeight(heightDiff, heightChangeSpeed);
    }

    /// <summary>
    /// 飛行する高さを変える
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
            // 目標の高さより高い場合
            if (currentFlyHeight == flyHeightLimit.min) return true;
            currentFlyHeight -= changeHeightValue;
        }
        else
        {
            // 今日の高さより低い場合
            if (currentFlyHeight == flyHeightLimit.max) return true;
            currentFlyHeight += changeHeightValue;
        }

        currentFlyHeight = Mathf.Clamp(currentFlyHeight, flyHeightLimit.min, flyHeightLimit.max);
        agent.baseOffset = currentFlyHeight;

        return false;
    }
}
