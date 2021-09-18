using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    #region variables
    [SerializeField]
    private Transform cameraTrans = null;
    [SerializeField]
    private PlayerAttackCollider meleeAttackCollider = null;
    [SerializeField]
    private MagicBullet magicBullet = null;
    [SerializeField]
    private PlayerCamera playerCamera = null;
    [SerializeField]
    private GameObject healEffect = null;
    [SerializeField]
    private DrawObject myDrawObject = null;

    /// <summary>
    /// 回避移動スピードのイージング
    /// </summary>
    [SerializeField]
    private AnimationCurve avoidMoveSpeedCurve = null;

    /// <summary>
    /// 物理挙動で回転させるか
    /// </summary>
    [SerializeField]
    private bool isDynamicRotation = false;
    /// <summary>
    /// プレイヤーの回転速度
    /// </summary>
    [SerializeField]
    private float rotateSpeed = 2.0f;
    /// <summary>
    /// 通常時の最大移動スピード
    /// </summary>
    [SerializeField]
    private float normalMaxMoveSpeed = 5.0f;
    /// <summary>
    /// ダッシュ中の最大移動スピード
    /// </summary>
    [SerializeField]
    private float runMaxMoveSpeed = 10.0f;
    /// <summary>
    /// エイム中の最大スピード
    /// </summary>
    [SerializeField]
    private float aimingMaxMoveSpeed = 2.0f;
    /// <summary>
    /// 移動の加速度
    /// </summary>
    [SerializeField]
    private float moveAcceleration = 100.0f;
    /// <summary>
    /// ジャンプのスピード
    /// </summary>
    [SerializeField]
    private float jumpSpeed = 5.0f;
    /// <summary>
    /// 魔法弾のスピード
    /// </summary>
    [SerializeField]
    private float magicSpeed = 20.0f;
    /// <summary>
    /// ダメージを受けた際の無敵時間
    /// </summary>
    [SerializeField]
    private float damegeInterval = 1.0f;
    /// <summary>
    /// 魔法弾が消えるまでの時間
    /// </summary>
    [SerializeField]
    private float magicBulletLifeTime = 5.0f;
    /// <summary>
    /// 回避する時のスピード
    /// </summary>
    [SerializeField]
    private float avoidSpeed = 20.0f;
    /// <summary>
    /// 回避移動をする時間
    /// </summary>
    [SerializeField]
    private float avoidMoveTime = 0.2f;
    /// <summary>
    /// 次の回避のインターバル
    /// </summary>
    [SerializeField]
    private float avoidInterval = 0.5f;
    /// <summary>
    /// 初期の灰色に塗る範囲
    /// </summary>
    [SerializeField]
    private float drawRadius = 1.0f;
    /// <summary>
    /// 魔法弾の威力
    /// </summary>
    [SerializeField]
    private int magicAttackPower = 20;
    /// <summary>
    /// 魔法を撃つ時のインク消費量
    /// </summary>
    [SerializeField]
    private int magicInkCost = 50;
    /// <summary>
    /// 回避のインク消費量
    /// </summary>
    [SerializeField]
    private int avoidInkCost = 10;
    /// <summary>
    /// 回復魔法のインク消費量
    /// </summary>
    [SerializeField]
    private int healInkCost = 500;
    /// <summary>
    /// 回復の魔法のHP回復量
    /// </summary>
    [SerializeField]
    private int healHpValue = 50;
    /// <summary>
    /// 近接攻撃1のダメージ量
    /// </summary>
    [SerializeField]
    private int meleeAttack1Power = 5;
    /// <summary>
    /// 近接攻撃2のダメージ量
    /// </summary>
    [SerializeField]
    private int meleeAttack2Power = 10;
    /// <summary>
    /// 近接攻撃3のダメージ量
    /// </summary>
    [SerializeField]
    private int meleeAttack3Power = 15;

    private Rigidbody playerRb = null;
    private CapsuleCollider playerCollider = null;
    private AfterImageGenerator afterImageGenerator = null;

    /// <summary>
    /// 入力されている移動方向
    /// </summary>
    private Vector2 inputMoveVec = new Vector2();
    /// <summary>
    /// プレイヤーが動いている方向
    /// </summary>
    private Vector3 currentMoveVec = new Vector3();

    /// <summary>
    /// ダッシュ中か
    /// </summary>
    private bool isDash = true;
    /// <summary>
    /// 接地しているか
    /// </summary>
    private bool isGround = false;
    /// <summary>
    /// エイム中か
    /// </summary>
    private bool isAiming = false;
    /// <summary>
    /// エイムを行うバインドが有効かどうか
    /// </summary>
    private bool isActivateAimBind = false;
    /// <summary>
    /// 回避中か
    /// </summary>
    private bool isAvoiding = false;
    /// <summary>
    /// 近接攻撃中か
    /// </summary>
    private bool isMeleeAttacking = false;
    /// <summary>
    /// 魔法攻撃中か
    /// </summary>
    private bool isMagicAttacking = false;
    /// <summary>
    /// 行動が出来なくなるアニメーションが再生中か
    /// </summary>
    private bool isPlayingUnableToMoveAnim = false;
    /// <summary>
    /// 連続する次の近接攻撃が有効になっているか
    /// </summary>
    private bool isEnableNextMeleeAttack = false;
    /// <summary>
    /// 攻撃中の回転が出来るか
    /// </summary>
    private bool canAttackingRotate = false;
    /// <summary>
    /// 近接攻撃の連続攻撃回数
    /// </summary>
    private int meleeAttackingCount = 0;
    /// <summary>
    /// 現在の移動スピード
    /// </summary>
    private float currentMoveSpeed = 0;
    /// <summary>
    /// 現在の残り無敵時間
    /// </summary>
    private float currentDamageInterval = 0;
    /// <summary>
    /// 現在の回避中の時間
    /// </summary>
    private float currentAvoidingTime = 0;
    /// <summary>
    /// 現在の回避インターバル
    /// </summary>
    private float currentAvoidInterval = 0;
    /// <summary>
    /// Y軸の最大上昇スピード
    /// </summary>
    private float maxYAxisVelocity = 5;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        afterImageGenerator = GetComponent<AfterImageGenerator>();

        SetHpToMax();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamageInterval();
        UpdateAvoidInterval();
        CheckGround();
        UpdatePlayerMove();
        UpdatePlayerAnim();
        UpdateDrawRange();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMoveVec =  InputValueConverter.GetMoveValue(context);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!IsControlable()) return;

        isDash = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsControlable()) return;

        Jump();
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (!IsControlable()) return;

        if (context.performed) MeleeAttack();
    }

    public void OnMagicAttack(InputAction.CallbackContext context)
    {
        if (!IsControlable()) return;

        if (context.performed) MagicAttack();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        isActivateAimBind = context.performed;

        if (!IsControlable() || isAttacking()) return;

        ChangeAnimManage(context.performed);
    }

    public void OnAvoid(InputAction.CallbackContext context)
    {
        if (!IsControlable() || isAttacking()) return;

        if (context.performed) Avoid();
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!IsControlable()) return;

        if (context.performed) Heal();
    }

    /// <summary>
    /// プレイヤーの移動を更新する
    /// </summary>
    private void UpdatePlayerMove()
    {
        float speedLimit = 0;
        // TODO: プレイヤーの動き自体を改修したい
        // 斜面に当たったなどして上に吹っ飛ばないようにYベクトルを0にする
        if (playerRb.velocity.y > maxYAxisVelocity)
        {
            playerRb.velocity = currentMoveVec;
        }

        if (isAvoiding)
        {
            UpdateAvoidMove();
            return;
        }

        if (!isDynamicRotation) {
            // 回転が滑らないように回転ベクトルを0にする
            playerRb.angularVelocity = Vector3.zero;
        }

        // スピードの制限を設定する
        if (inputMoveVec.magnitude > 0 && IsControlable() && !isAttacking())
        {
            if (isAiming)
            {
                speedLimit = aimingMaxMoveSpeed;
            }
            else if (isDash)
            {
                speedLimit = runMaxMoveSpeed;
            }
            else
            {
                speedLimit = normalMaxMoveSpeed;
            }
        }
        // スピード制限より早い場合は減速させる
        if (currentMoveSpeed > speedLimit)
        {
            currentMoveSpeed -= moveAcceleration * Time.deltaTime;
            if (currentMoveSpeed < speedLimit)
            {
                currentMoveSpeed = speedLimit;
            }
        }

        // エイム時はカメラの向いている方向に向く
        if (isAiming)
        {
            Vector3 cameraDir = cameraTrans.forward;
            cameraDir.y = 0;
            Quaternion cameraRotaion = Quaternion.LookRotation(cameraDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraRotaion, rotateSpeed * Time.deltaTime);
        }

        // 回転できる攻撃中の場合は回転させる
        if (isAttacking() && canAttackingRotate)
        {
            Vector3 direction;
            if (inputMoveVec == Vector2.zero)
            {
                direction = transform.forward;
            }
            else
            {
                direction = ConvertCameraForwardToQuaternion() * ConvertInputVecToMoveVec(inputMoveVec);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * TimeUtil.GetDeltaTime());
        }

        // 入力がない場合は回転処理や加速をさせない
        if (inputMoveVec.magnitude == 0 || !IsControlable() || isAttacking())
        {
            SetMoveVelocity(currentMoveVec * currentMoveSpeed);
            return;
        }
        else if (!isAiming)
        {
            // 入力した方向にプレイヤーを回転させる
            RotateTowardsDirection(inputMoveVec);
        }

        // 加速させる
        if (currentMoveSpeed < speedLimit)
        {
            currentMoveSpeed += moveAcceleration * Time.deltaTime;
            if (currentMoveSpeed > speedLimit)
            {
                currentMoveSpeed = speedLimit;
            }
        }

        Vector3 moveDir = isAiming ? transform.rotation * ConvertInputVecToMoveVec(inputMoveVec) : transform.forward;
        Vector3 moveVec = moveDir * currentMoveSpeed;
        SetMoveVelocity(moveVec);
    }

    /// <summary>
    /// 接地しているか判定する
    /// </summary>
    private void CheckGround()
    {
        RaycastHit raycastHit;
        float rayRadius = playerCollider.radius;
        float rayDistance = CalcDistanceForFoot();
        // SphereCastを発射し、地面に当たった場合は接地中と判定する
        isGround = Physics.SphereCast(GetCenterPos(), rayRadius, Vector3.down, out raycastHit, rayDistance, LayerMaskUtil.GetLayerMaskGrounds());
    }

    /// <summary>
    /// ジャンプさせる
    /// </summary>
    private void Jump()
    {
        if (!isGround) return;

        Vector3 playerVec = playerRb.velocity;
        playerVec.y = jumpSpeed;

        playerRb.velocity = playerVec;
    }

    /// <summary>
    /// 近接攻撃
    /// </summary>
    private void MeleeAttack()
    {
        if (isAiming) return;

        //if (isAttacking) return;

        if (meleeAttackingCount == (int)PlayerMeleeAttackState.NotPlaying)
        {
            PlayNextMeleeAttack();
        }
        else
        {
            isEnableNextMeleeAttack = true;
        }

        //animator.SetTrigger("MeleeAttack");
        //StartCoroutine(WatchAttackAnimState("MeleeAttack"));
    }

    /// <summary>
    /// 連続する近接攻撃の次の攻撃を再生する
    /// </summary>
    private void PlayNextMeleeAttack()
    {
        OnAttackColliderDisable();
        isMeleeAttacking = true;

        if (meleeAttackingCount == (int)PlayerMeleeAttackState.NotPlaying)
        {
            animator.SetTrigger("MeleeAttack_1");
            meleeAttackingCount = (int)PlayerMeleeAttackState.Melee1Playing;
            meleeAttackCollider.damageValue = meleeAttack1Power;

            AudioManager.Instance.PlayRandomPitchSE("ere_light_attack");
        }
        else if (meleeAttackingCount == (int)PlayerMeleeAttackState.Melee1Playing)
        {
            animator.SetTrigger("MeleeAttack_2");
            meleeAttackingCount = (int)PlayerMeleeAttackState.Melee2Playing;
            meleeAttackCollider.damageValue = meleeAttack2Power;

            AudioManager.Instance.PlayRandomPitchSE("ere_light_attack");
        }
        else if (meleeAttackingCount == (int)PlayerMeleeAttackState.Melee2Playing)
        {
            animator.SetTrigger("MeleeAttack_3");
            meleeAttackingCount = (int)PlayerMeleeAttackState.Melee3Playing;
            meleeAttackCollider.damageValue = meleeAttack3Power;

            AudioManager.Instance.PlayRandomPitchSE("ere_heavy_attack");
        }

        isEnableNextMeleeAttack = false;
    }

    /// <summary>
    /// 魔法攻撃
    /// </summary>
    private void MagicAttack()
    {
        // インクが足りなかったら魔法を出さない
        if (!GameSceneUIManager.Instance.InkGauge.IsEnoughInk(magicInkCost)) return;

        if (isAttacking()) return;

        Vector3 shotOriginPos = GetCenterPos();
        PlayerMagicBullet magicBullet = GameSceneManager.Instance.GetPlayerMagicBulletPool().GetObject<PlayerMagicBullet>();
        int attackPower = (int)(magicAttackPower * GameSceneManager.Instance.RedLevelTable.GetProportionByItemType(ItemType.AttackUp));

        if (isAiming)
        {
            Vector3 aimPos = GameSceneUIManager.Instance.Reticle.GetReticleRayHitPos();
            Vector3 aimDirection = (aimPos - shotOriginPos).normalized;

            magicBullet.Shot(shotOriginPos, aimDirection * magicSpeed, attackPower, magicBulletLifeTime);
        }
        else
        {
            magicBullet.Shot(shotOriginPos, transform.forward * magicSpeed, attackPower, magicBulletLifeTime);
        }

        animator.SetTrigger("MagicAttack");
        GameSceneUIManager.Instance.InkGauge.AddInk(-magicInkCost);

        AudioManager.Instance.PlayRandomPitchSE("ere_light_attack");

        isMagicAttacking = true;
    }

    /// <summary>
    /// 回避行動を行う
    /// </summary>
    private void Avoid()
    {
        // 回避待機時間が残っている場合は回避させない
        if (currentAvoidInterval > 0) return;
        // 空中にいる場合は回避させない
        if (isAvoiding || !isGround) return;

        // インクが足りなかったら回避させない
        if (!GameSceneUIManager.Instance.InkGauge.IsEnoughInk(avoidInkCost)) return;

        Vector3 direction;
        if (inputMoveVec == Vector2.zero)
        {
            direction = transform.forward;
        }
        else
        {
            direction = ConvertCameraForwardToQuaternion() * ConvertInputVecToMoveVec(inputMoveVec);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        SetMoveVelocity(avoidSpeed * direction);

        currentMoveSpeed = 0;
        currentAvoidingTime = 0;
        currentAvoidInterval = avoidInterval;

        GameSceneUIManager.Instance.InkGauge.AddInk(-avoidInkCost);
        // 残像の生成を開始する
        afterImageGenerator.StartGenerate();
        //プレイヤーの表示を消す
        SetEnableRenderers(false);

        isAvoiding = true;
    }

    /// <summary>
    /// 回復を行う
    /// </summary>
    public void Heal()
    {
        if (currentHp >= maxHp) return;
        if (!GameSceneUIManager.Instance.InkGauge.IsEnoughInk(healInkCost)) return;

        animator.SetTrigger("Heal");
    }

    /// <summary>
    /// 回避中の移動処理
    /// </summary>
    private void UpdateAvoidMove()
    {
        currentAvoidingTime = Mathf.Clamp(currentAvoidingTime + Time.deltaTime, 0, avoidMoveTime);
        float currentAvoidSpeed = avoidSpeed * avoidMoveSpeedCurve.Evaluate(currentAvoidingTime / avoidMoveTime);

        SetMoveVelocity(currentMoveVec * currentAvoidSpeed);

        if (currentAvoidingTime >= avoidMoveTime)
        {
            // プレイヤーの表示をもどす
            SetEnableRenderers(true);
            // 残像の生成を止める
            afterImageGenerator.StopGenerate();
            isAvoiding = false;
        }
    }

    /// <summary>
    /// エイム状態を切り替える
    /// </summary>
    /// <param name="isAiming"></param>
    private void ChangeAnimManage(bool isAiming)
    {
        if (this.isAiming == isAiming) return;

        GameSceneUIManager.Instance.Reticle.OnAim(isAiming);
        playerCamera.OnAim(isAiming);
        animator.SetBool("Aiming", isAiming);
        // エイム状態になった時にOnAimトリガーを発火する
        if (isAiming)
        {
            animator.SetTrigger("OnAim");
        }

        this.isAiming = isAiming;
    }

    /// <summary>
    /// 移動ベクトルをRigidBodyのVelocityに適用する
    /// </summary>
    /// <param name="moveVec"></param>
    private void SetMoveVelocity(Vector3 moveVec)
    {
        moveVec.y = playerRb.velocity.y;
        playerRb.velocity = moveVec;

        currentMoveVec = moveVec.normalized;
    }

    /// <summary>
    /// カメラの前方方向から指定した方向へ回転させる(二次元方向)
    /// </summary>
    /// <param name="rotateDirection"></param>
    private void RotateTowardsDirection(Vector2 rotateDirection)
    {
        RotateTowardsDirection(ConvertInputVecToMoveVec(rotateDirection));
    }

    /// <summary>
    /// カメラの前方方向から指定した方向へ回転させる
    /// </summary>
    /// <param name="rotateDirection"></param>
    private void RotateTowardsDirection(Vector3 rotateDirection)
    {
        Quaternion cameraRotaion = ConvertCameraForwardToQuaternion();
        Quaternion inputRotaion = Quaternion.LookRotation(rotateDirection);
        Quaternion playerRotation = cameraRotaion * inputRotaion;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerRotation, rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// カメラの前方方向のクオータニオンを取得うすｒ
    /// </summary>
    /// <returns></returns>
    private Quaternion ConvertCameraForwardToQuaternion()
    {
        Vector3 cameraDirection = cameraTrans.forward;
        cameraDirection.y = 0;

        return Quaternion.LookRotation(cameraDirection);
    }

    /// <summary>
    /// 二次元入力を移動ベクトルに変換する
    /// </summary>
    /// <param name="inputVec"></param>
    /// <returns></returns>
    private Vector3 ConvertInputVecToMoveVec(Vector2 inputVec)
    {
        return Vector3.right * inputVec.x + Vector3.forward * inputVec.y;
    }

    /// <summary>
    /// プレイヤーの中心の座標を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPos()
    {
        return transform.position + playerCollider.center;
    }

    /// <summary>
    /// ダメージを受けた時の処理
    /// </summary>
    /// <param name="attackPower"></param>
    public override void TakeDamage(int attackPower)
    {
        // HPがない場合はダメージを受けなくする
        if (isDead) return;
        // 回避中はダメージを受けなくする
        if (isAvoiding) return;
        // ゲームクリア時はダメージを受けなくする
        if (GameSceneManager.Instance.isGameClear) return;
        if (currentDamageInterval > 0) return;

        base.TakeDamage(attackPower);

        currentDamageInterval = damegeInterval;
        GameSceneUIManager.Instance.PlayerHPGauge.SetFillAmountByHp(maxHp, currentHp);

        animator.SetTrigger("Damage");
        // エイム状態は解除する
        ChangeAnimManage(false);
        ResetMeleeAttacking();
        OnAttackColliderDisable();

        AudioManager.Instance.PlayRandomPitchSE("ere_damage");

        isMagicAttacking = false;
    }

    /// <summary>
    /// 倒された際の処理
    /// </summary>
    protected override void OnDead()
    {
        isAiming = false;
        isAvoiding = false;
        isDash = false;

        inputMoveVec = Vector2.zero;
        GameSceneUIManager.Instance.Reticle.OnAim(isAiming);
        playerCamera.OnAim(isAiming);

        animator.SetTrigger("Dead");

        GameSceneManager.Instance.ShowGameOver();
    }

    /// <summary>
    /// 無敵時間を更新する
    /// </summary>
    public void UpdateDamageInterval()
    {
        currentDamageInterval = Mathf.Max(currentDamageInterval - Time.deltaTime, 0);
    }

    /// <summary>
    /// 回避時間を更新する
    /// </summary>
    public void UpdateAvoidInterval()
    {
        currentAvoidInterval = Mathf.Max(currentAvoidInterval - Time.deltaTime, 0);
    }

    /// <summary>
    /// プレイヤーのアニメーションを更新する
    /// </summary>
    public void UpdatePlayerAnim()
    {
        if (inputMoveVec != Vector2.zero && isGround && !isAiming && !isAvoiding)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        animator.SetBool("Dash", isDash);
        animator.SetBool("Avoid", isAvoiding);
    }

    public void UpdateDrawRange()
    {
        float currentDrawRadius = drawRadius * GameSceneManager.Instance.BlueLevelTable.GetProportionByItemType(ItemType.RangeUp);
        myDrawObject.drawRadius = currentDrawRadius;
        // スケールは直径なので半径の2倍
        myDrawObject.transform.localScale = Vector3.one * currentDrawRadius * 2;
    }

    /// <summary>
    /// 操作が可能な状態かどうか
    /// </summary>
    private bool IsControlable()
    {
        return !isDead && !isPlayingUnableToMoveAnim && !GameSceneManager.Instance.isGamePausing();
    }

    private bool isAttacking()
    {
        return isMeleeAttacking || isMagicAttacking;
    }

    /// <summary>
    /// 近接攻撃の状態をリセットする
    /// </summary>
    private void ResetMeleeAttacking()
    {
        isMeleeAttacking = false;
        meleeAttackingCount = (int)PlayerMeleeAttackState.NotPlaying;
    }

    /// <summary>
    /// アニメーションのトリガーをリセットする
    /// </summary>
    private void ResetAnimTrigger()
    {
        animator.ResetTrigger("MeleeAttack_1");
        animator.ResetTrigger("MeleeAttack_2");
        animator.ResetTrigger("MeleeAttack_3");
        animator.ResetTrigger("OnAim");
        animator.ResetTrigger("Damage");
        animator.ResetTrigger("Heal");
        animator.ResetTrigger("MagicAttack");
    }

    /// <summary>
    /// 足元までの長さを計算する
    /// </summary>
    /// <returns></returns>
    private float CalcDistanceForFoot()
    {
        // 接地判定を安定させるための余分な長さ
        float heightMargin = 0.2f;
        float rayRadius = playerCollider.radius;
        float rayDistance = playerCollider.height / 2 - rayRadius + heightMargin;

        return rayDistance;
    }

    /// <summary>
    /// 足音を鳴らす
    /// </summary>
    private void PlayFootsteps()
    {
        RaycastHit raycastHit;
        float rayRadius = playerCollider.radius;
        float rayDistance = CalcDistanceForFoot();
        if (!Physics.SphereCast(GetCenterPos(), rayRadius, Vector3.down, out raycastHit, rayDistance, LayerMaskUtil.GetLayerMaskFootstepsObject())) return;

        FootstepRinger footstepRinger = raycastHit.transform.GetComponentInParent<FootstepRinger>();

        if (footstepRinger == null) return;

        footstepRinger.PlayFootsteps(raycastHit.textureCoord, transform.position);
    }

    /// <summary>
    /// 近接攻撃判定を有効にする (アニメーションイベント用)
    /// </summary>
    private void OnAttackColliderEneble()
    {
        meleeAttackCollider.EnableCollider();
    }

    /// <summary>
    /// 近接攻撃判定を無効にする (アニメーションイベント用)
    /// </summary>
    private void OnAttackColliderDisable()
    {
        meleeAttackCollider.DisableCollider();
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

    /// <summary>
    /// 回復魔法を詠唱した時のイベント (アニメーションイベント用)
    /// </summary>
    private void OnCastHealMagic()
    {
        if (isDead) return;

        int addHPValue = (int)(healHpValue * GameSceneManager.Instance.GreenLevelTable.GetProportionByItemType(ItemType.HealingUp));
        currentHp = Mathf.Clamp(currentHp += addHPValue, 0, maxHp);

        GameSceneUIManager.Instance.PlayerHPGauge.SetFillAmountByHp(maxHp, currentHp);
        GameSceneUIManager.Instance.InkGauge.AddInk(-healInkCost);
        AudioManager.Instance.PlaySE("Buff 3", GetCenterPos());

        GameObject healEffectObj = Instantiate(healEffect, transform);
        // エフェクトは1秒後に破棄する
        Destroy(healEffectObj, 1.0f);
    }

    private void OnReachNextMeleeAttack()
    {
        if (!isEnableNextMeleeAttack) return;

        PlayNextMeleeAttack();
    }

    /// <summary>
    /// 近接攻撃アニメーションが終了した時のイベント (アニメーションイベント用)
    /// </summary>
    private void OnFinishMeleeAttack()
    {
        ResetMeleeAttacking();
    }

    /// <summary>
    /// 魔法攻撃アニメーションが終了した時のイベント (アニメーションイベント用)
    /// </summary>
    private void OnFinishMagicAttack()
    {
        isMagicAttacking = false;
        ChangeAnimManage(isActivateAimBind);
    }

    /// <summary>
    /// 攻撃中に回転できるようにする (アニメーションイベント用)
    /// </summary>
    private void OnEnableRotateAttack()
    {
        canAttackingRotate = true;
    }

    /// <summary>
    /// 攻撃中に回転できなくする (アニメーションイベント用)
    /// </summary>
    private void OnDisableRotateAttack()
    {
        canAttackingRotate = false;
    }

    /// <summary>
    /// モーション中に足が地面についた時のイベント (アニメーションイベント用)
    /// </summary>
    private void OnFootsteps()
    {
        PlayFootsteps();
    }
}

public enum PlayerMeleeAttackState
{
    NotPlaying,
    Melee1Playing,
    Melee2Playing,
    Melee3Playing
}