using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Enemy : Character, IReticleReactable
{
    /// <summary>
    /// 地形の色を変える処理を有効化する距離
    /// </summary>
    protected static readonly float drawComponentEnableDistance = 50.0f;

    [SerializeField]
    protected EnemyUI enemyHpGauge = null;
    [SerializeField]
    protected DrawObject drawComponent = null;
    [SerializeField]
    private ExplodeObject explodeObject = null;
    [SerializeField]
    protected Transform eyeTrans = null;
    [SerializeField]
    protected Transform rootTrans = null;
    [SerializeField]
    protected Transform outLineTrans = null;
    [SerializeField]
    protected UnityEvent onDeadEvent = new UnityEvent();

    [SerializeField]
    protected bool canFlinch = false;
    [SerializeField]
    protected bool igroneObstacle = false;
    [SerializeField]
    protected float detectRadius = 5.0f;
    [SerializeField, Range(0, 180)]
    protected float detectAngle = 80.0f;
    [SerializeField]
    protected float loseSightRadius = 7.0f;
    [SerializeField]
    protected float cautionTime = 5.0f;
    [SerializeField]
    protected float flinchTime = 0.5f;
    [SerializeField, Range(0, 1)]
    protected float flinchRate = 1;

    protected Player detectPlayer = null;
    protected Rigidbody myRb = null;
    protected EnemySearchStatus searchState = EnemySearchStatus.undetected;

    protected bool isDetectPlayer = false;
    protected bool isPlayingWaitAnim = false;
    protected float cautionDetectAngle = 180.0f;
    protected float currentCautionTime = 0;
    protected float currentFlinchTime = 0;
    protected float currentDamageEffectTime = 0;
    protected float damageEffectTime = 0.5f;

    private Collider[] searchBuffer = new Collider[10];

    // Start is called before the first frame update
    protected void Start()
    {
        myRb = GetComponent<Rigidbody>();
        detectPlayer = Player.GetPlayer();

        SetHpToMax();
        ChangeEnemySearchStatus(EnemySearchStatus.undetected);
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateDrawComponent();
        UpdateSearchState();
        UpdateCautioning();
        UpdateFlinching();
        UpdateDamageEffect();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="attackPower"></param>
    public override void TakeDamage(int attackPower)
    {
        if (!this.enabled) return;
        if (isDead) return;

        base.TakeDamage(attackPower);

        UpdateHpGauge();
        // 警戒状態にする時間を設定する
        SetCaution();
        SetFlinch();
        currentDamageEffectTime = damageEffectTime;
    }

    /// <summary>
    /// 倒された際の処理
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();

        if (drawComponent != null)
        {
            drawComponent.enabled = false;
        }

        SetLayerForBodyColliders(LayerMaskUtil.DeadEnemyLayerNumber);
        StartCoroutine(PlayDeadAnim());

        enemyHpGauge?.HideHp();

        MissionManager.Instance.AddCountOfMission(MissionName.killEnemies, 1);

        onDeadEvent.Invoke();
    }

    /// <summary>
    /// エイムが合わさった時の処理
    /// </summary>
    /// <param name="raycastHit"></param>
    public void OnAimed(RaycastHit raycastHit)
    {
        if (!this.enabled) return;

        SetActiveOutLine(true);
    }

    /// <summary>
    /// エイムが外れた時の処理
    /// </summary>
    public void OnUnAimed()
    {
        SetActiveOutLine(false);
    }

    /// <summary>
    /// 地形描画コンポーネントを更新する
    /// </summary>
    protected void UpdateDrawComponent()
    {
        bool enableDraw = TransformUtil.Calc2DDistance(detectPlayer.transform.position, transform.position) <= drawComponentEnableDistance;

        if (drawComponent == null) return;
        if (drawComponent.enabled == enableDraw) return;

        drawComponent.enabled = enableDraw;
    }

    /// <summary>
    /// 敵のHPゲージを更新する
    /// </summary>
    protected void UpdateHpGauge()
    {
        if (enemyHpGauge == null) return;

        enemyHpGauge.UpdateGaugeFillAmount(currentHp, maxHp);

        if (isDead) return;

        enemyHpGauge.ShowHp();
    }

    /// <summary>
    /// 警戒状態の更新を行う
    /// </summary>
    protected void UpdateCautioning()
    {
        currentCautionTime = Mathf.Max(0, currentCautionTime - TimeUtil.GetDeltaTime());
    }

    /// <summary>
    /// ひるみ状態の更新を行う
    /// </summary>
    protected void UpdateFlinching()
    {
        currentFlinchTime = Mathf.Max(0, currentFlinchTime - TimeUtil.GetDeltaTime());

        if (animator == null) return;

        animator.SetBool("Flinch", IsFlinching());
    }

    /// <summary>
    /// 警戒状態かどうか
    /// </summary>
    /// <returns></returns>
    public bool IsCautioning()
    {
        return currentCautionTime > 0;
    }

    /// <summary>
    /// ひるみ状態かどうか
    /// </summary>
    public bool IsFlinching()
    {
        return currentFlinchTime > 0;
    }

    /// <summary>
    /// 検知状態からフレーム更新時の処理を分けて行う
    /// </summary>
    protected void UpdateSearchState()
    {
        if (isDead) return;

        if (IsFlinching())
        {
            ActionForFlinching();
            return;
        }

        if (searchState == EnemySearchStatus.undetected)
        {
            ActionForUndetected();
        }
        else if (searchState == EnemySearchStatus.detected)
        {
            ActionForDetected();
        }
        else if (searchState == EnemySearchStatus.loseSight)
        {
            ActionForLoseSight();
        }
    }

    /// <summary>
    /// 未発見時の行動
    /// </summary>
    protected virtual void ActionForUndetected()
    {
        if (CheckDetectPlayer())
        {
            ChangeEnemySearchStatus(EnemySearchStatus.detected);
        }
    }

    /// <summary>
    /// 発見時の行動
    /// </summary>
    protected virtual void ActionForDetected()
    {
        if (CheckLoseSightPlayer())
        {
            ChangeEnemySearchStatus(EnemySearchStatus.loseSight);
        }
    }

    /// <summary>
    /// 見失った時の行動
    /// </summary>
    protected virtual void ActionForLoseSight()
    {
        ChangeEnemySearchStatus(EnemySearchStatus.undetected);
    }

    /// <summary>
    /// ひるみ中の行動
    /// </summary>
    protected virtual void ActionForFlinching()
    {

    }

    /// <summary>
    /// 検知状態を変更する
    /// </summary>
    protected void ChangeEnemySearchStatus(EnemySearchStatus status)
    {
        searchState = status;

        if (searchState == EnemySearchStatus.undetected)
        {
            OnUndetected();
        }
        else if (searchState == EnemySearchStatus.detected)
        {
            OnDetected();
        }
        else if (searchState == EnemySearchStatus.loseSight)
        {
            OnLoseSight();
        }
    }

    /// <summary>
    /// 見失った直後に行う処理
    /// </summary>
    protected virtual void OnUndetected() { }

    /// <summary>
    /// 発見した直後に行う処理
    /// </summary>
    protected virtual void OnDetected() { }

    /// <summary>
    /// 見失った直後に行う処理
    /// </summary>
    protected virtual void OnLoseSight() { }

    /// <summary>
    /// プレイヤーが検知内にいるか判定する
    /// </summary>
    /// <returns></returns>
    protected bool CheckDetectPlayer()
    {
        Vector3 enemyPos = transform.position;
        // プレイヤーが検知する範囲内にいるか
        int detectCount = Physics.OverlapSphereNonAlloc(enemyPos, detectRadius, searchBuffer, 1 << LayerMaskUtil.PlayerLayerNumber);

        if (detectCount <= 0) return false;

        Player player = null;
        foreach (Collider detectCollider in searchBuffer)
        {
            if (detectCollider.CompareTag(TagUtil.PlayerTagName))
            {
                player = detectCollider.GetComponent<Player>();
                break;
            }
        }

        if (player == null) return false;

        Vector3 playerPos = player.GetCenterPos();
        Vector3 playerPosDiff = playerPos - enemyPos;
        // 警戒中なら全方位から気づくようにする
        float cunnretDetectAngle = IsCautioning() ? cautionDetectAngle : detectAngle;
        // プレイヤーが正面から見て検知する角度外なら検知しない
        if (Vector3.Angle(transform.forward, playerPosDiff) > cunnretDetectAngle) return false;

        if (!igroneObstacle)
        {
            // プレイヤーと敵間に障害物がある場合は検知しない
            if (Physics.Raycast(GetEyePos(), playerPosDiff, playerPosDiff.magnitude, LayerMaskUtil.GetLayerMaskGrounds())) return false;
        }

        detectPlayer = player;

        return true;
    }

    /// <summary>
    /// プレイヤーを見失うか判定する
    /// </summary>
    /// <returns></returns>
    protected bool CheckLoseSightPlayer()
    {
        if (detectPlayer == null) return false;

        Vector3 enemyPos = transform.position;
        // プレイヤーが見失う範囲外にいるか
        int detectCount = Physics.OverlapSphereNonAlloc(enemyPos, loseSightRadius, searchBuffer, LayerMask.GetMask(LayerMaskUtil.PlayerLayerName));

        if (detectCount <= 0) return true;
        // プレイヤーがまだ範囲内にいるなら障害物の判定を行う
        Vector3 playerPos = detectPlayer.transform.position;
        Vector3 playerPosDiff = playerPos - enemyPos;

        if (!igroneObstacle)
        {
            // プレイヤーと敵間に障害物がある場合は見失ったとする
            if (Physics.Raycast(GetEyePos(), playerPosDiff, playerPosDiff.magnitude, LayerMaskUtil.GetLayerMaskGrounds())) return true;
        }

        return false;
    }

    /// <summary>
    /// 目の座標を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEyePos()
    {
        // 目のTransformがない場合は敵の座標を返す
        if (eyeTrans == null)
        {
            return transform.position;
        }

        return eyeTrans.position;
    }

    /// <summary>
    /// 検知したプレイヤーに向けて徐々に回転する
    /// </summary>
    /// <param name="rotateSpeed"></param>
    /// <returns></returns>
    protected Quaternion RotateTowardsToDetectPlayer(float rotateSpeed, bool igroneYAxis = true)
    {
        if (detectPlayer == null) return Quaternion.identity;

        Vector3 playerPos = detectPlayer.transform.position;
        return RotateTowardsToPosition(playerPos, rotateSpeed, igroneYAxis);
    }

    /// <summary>
    /// 特定の座標に向けて徐々に回転させる
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="igroneYAxis"></param>
    /// <returns></returns>
    protected Quaternion RotateTowardsToPosition(Vector3 position, float rotateSpeed, bool igroneYAxis = true)
    {
        Vector3 myPos = transform.position;
        // Y軸を無視する場合はY座標を0として計算する
        if (igroneYAxis)
        {
            position.y = 0;
            myPos.y = 0;
        }
        Vector3 rotateDirection = position - myPos;

        return Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotateDirection), rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 正面にプレイヤーがいるか
    /// </summary>
    /// <param name="detectAngleLimit"></param>
    /// <returns></returns>
    protected bool IsTherePlayerByforward(float detectAngleLimit)
    {
        if (detectPlayer == null) return false;

        Vector3 playerPos = detectPlayer.transform.position;
        // プレイヤーが正面から見て特定の角度内か判定する
        return IsTherePositionByforward(playerPos, detectAngleLimit);
    }

    /// <summary>
    /// 特定の座標が正面にあるか
    /// </summary>
    /// <param name="position"></param>
    /// <param name="detectAngleLimit"></param>
    /// <returns></returns>
    protected bool IsTherePositionByforward(Vector3 position, float detectAngleLimit)
    {
        Vector3 enemyPos = transform.position;
        // Y座標は考慮しない
        position.y = 0;
        enemyPos.y = 0;

        Vector3 playerPosDiff = position - enemyPos;
        // 特定の座標が正面から見て特定の角度内か判定する
        return Vector3.Angle(transform.forward, playerPosDiff) <= detectAngleLimit;
    }

    /// <summary>
    /// アニメーションが再生完了するまでフラグを立て続ける
    /// </summary>
    /// <param name="animName"></param>
    /// <param name="layerNumber"></param>
    /// <param name="onPlayedAction"></param>
    /// <returns></returns>
    public IEnumerator WatchPlayeringAnimState(string animName, int layerNumber = 0, Action onPlayedAction = null)
    {
        isPlayingWaitAnim = true;

        yield return AnimatorUtil.WaitForAnimByName(animator, animName, layerNumber);

        onPlayedAction?.Invoke();
        isPlayingWaitAnim = false;
    }

    /// <summary>
    /// 倒された時のアニメーションを再生する
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayDeadAnim()
    {
        //explodeObject.StartExplode();

        if (animator != null)
        {
            animator.SetTrigger(deadAnimName);
            yield return StartCoroutine(AnimatorUtil.WaitForAnimByName(animator, deadAnimName, onPlayingDeadAnim: (AnimatorStateInfo animState) => {
                OnPlayingDeadAnim(animState);
            }));
        }

        // 消える時にパーティクルを出す
        Vector3 particlePos = rootTrans == null ? transform.position : rootTrans.position;
        GameSceneManager.Instance.GetDeadExpParticlePool().GetObject<ParticleObject>().PlayOfPosition(particlePos);
        AudioManager.Instance.PlaySE("Magical Impact 30", rootTrans.position);

        explodeObject?.StartExplode();

        Destroy(gameObject);
    }

    /// <summary>
    /// 死亡時のアニメーションが再生されている時に呼ばれる処理
    /// </summary>
    /// <param name="animState"></param>
    protected void OnPlayingDeadAnim(AnimatorStateInfo animState)
    {
        float deadAnimTime = Mathf.Clamp01(animState.normalizedTime);
        foreach (Renderer renderer in myRenderers)
        {
            //段々と灰色にさせていく
            Material rendererMaterial = renderer.material;
            if (!rendererMaterial.HasProperty("_Threshold")) continue;

            rendererMaterial.SetFloat("_Threshold", deadAnimTime);
        }
    }

    /// <summary>
    /// ダメージを受けた際のエフェクトを更新する
    /// </summary>
    protected void UpdateDamageEffect()
    {
        if (currentDamageEffectTime <= 0) return;

        currentDamageEffectTime = Mathf.Clamp(currentDamageEffectTime - Time.deltaTime, 0, damageEffectTime);

        float damageEffectRate = currentDamageEffectTime / damageEffectTime;
        foreach (Renderer renderer in myRenderers)
        {
            // 赤色にする
            Material rendererMaterial = renderer.material;
            if (!rendererMaterial.HasProperty("_Color")) continue;

            rendererMaterial.SetColor("_Color", Color.Lerp(Color.white, Color.red, damageEffectRate));
        }
    }

    /// <summary>
    /// アウトラインの表示を切り替える
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveOutLine(bool active)
    {
        if (outLineTrans == null) return;

        outLineTrans.gameObject.SetActive(active);
    }

    /// <summary>
    /// 警戒状態にする
    /// </summary>
    public void SetCaution()
    {
        currentCautionTime = cautionTime;
    }

    /// <summary>
    /// ひるませる
    /// </summary>
    /// <returns></returns>
    public virtual bool SetFlinch()
    {
        if (!canFlinch) return false;
        if (isDead) return false;
        if (UnityEngine.Random.Range(0, 1.0f) > flinchRate) return false;

        currentFlinchTime = flinchTime;

        return true;
    }

    protected enum EnemySearchStatus
    {
        /// <summary>
        /// 未発見
        /// </summary>
        undetected,
        /// <summary>
        /// 発見済
        /// </summary>
        detected,
        /// <summary>
        /// 見失った
        /// </summary>
        loseSight,
    }
}

public enum EnemyType
{
    Mushroom = 1 << 0,
    Crab = 1 << 1,
    Tank = 1 << 2,
}
