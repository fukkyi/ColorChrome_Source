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
    /// ����ړ��X�s�[�h�̃C�[�W���O
    /// </summary>
    [SerializeField]
    private AnimationCurve avoidMoveSpeedCurve = null;

    /// <summary>
    /// ���������ŉ�]�����邩
    /// </summary>
    [SerializeField]
    private bool isDynamicRotation = false;
    /// <summary>
    /// �v���C���[�̉�]���x
    /// </summary>
    [SerializeField]
    private float rotateSpeed = 2.0f;
    /// <summary>
    /// �ʏ펞�̍ő�ړ��X�s�[�h
    /// </summary>
    [SerializeField]
    private float normalMaxMoveSpeed = 5.0f;
    /// <summary>
    /// �_�b�V�����̍ő�ړ��X�s�[�h
    /// </summary>
    [SerializeField]
    private float runMaxMoveSpeed = 10.0f;
    /// <summary>
    /// �G�C�����̍ő�X�s�[�h
    /// </summary>
    [SerializeField]
    private float aimingMaxMoveSpeed = 2.0f;
    /// <summary>
    /// �ړ��̉����x
    /// </summary>
    [SerializeField]
    private float moveAcceleration = 100.0f;
    /// <summary>
    /// �W�����v�̃X�s�[�h
    /// </summary>
    [SerializeField]
    private float jumpSpeed = 5.0f;
    /// <summary>
    /// ���@�e�̃X�s�[�h
    /// </summary>
    [SerializeField]
    private float magicSpeed = 20.0f;
    /// <summary>
    /// �_���[�W���󂯂��ۂ̖��G����
    /// </summary>
    [SerializeField]
    private float damegeInterval = 1.0f;
    /// <summary>
    /// ���@�e��������܂ł̎���
    /// </summary>
    [SerializeField]
    private float magicBulletLifeTime = 5.0f;
    /// <summary>
    /// ������鎞�̃X�s�[�h
    /// </summary>
    [SerializeField]
    private float avoidSpeed = 20.0f;
    /// <summary>
    /// ����ړ������鎞��
    /// </summary>
    [SerializeField]
    private float avoidMoveTime = 0.2f;
    /// <summary>
    /// ���̉���̃C���^�[�o��
    /// </summary>
    [SerializeField]
    private float avoidInterval = 0.5f;
    /// <summary>
    /// �����̊D�F�ɓh��͈�
    /// </summary>
    [SerializeField]
    private float drawRadius = 1.0f;
    /// <summary>
    /// ���@�e�̈З�
    /// </summary>
    [SerializeField]
    private int magicAttackPower = 20;
    /// <summary>
    /// ���@�������̃C���N�����
    /// </summary>
    [SerializeField]
    private int magicInkCost = 50;
    /// <summary>
    /// ����̃C���N�����
    /// </summary>
    [SerializeField]
    private int avoidInkCost = 10;
    /// <summary>
    /// �񕜖��@�̃C���N�����
    /// </summary>
    [SerializeField]
    private int healInkCost = 500;
    /// <summary>
    /// �񕜂̖��@��HP�񕜗�
    /// </summary>
    [SerializeField]
    private int healHpValue = 50;
    /// <summary>
    /// �ߐڍU��1�̃_���[�W��
    /// </summary>
    [SerializeField]
    private int meleeAttack1Power = 5;
    /// <summary>
    /// �ߐڍU��2�̃_���[�W��
    /// </summary>
    [SerializeField]
    private int meleeAttack2Power = 10;
    /// <summary>
    /// �ߐڍU��3�̃_���[�W��
    /// </summary>
    [SerializeField]
    private int meleeAttack3Power = 15;

    private Rigidbody playerRb = null;
    private CapsuleCollider playerCollider = null;
    private AfterImageGenerator afterImageGenerator = null;

    /// <summary>
    /// ���͂���Ă���ړ�����
    /// </summary>
    private Vector2 inputMoveVec = new Vector2();
    /// <summary>
    /// �v���C���[�������Ă������
    /// </summary>
    private Vector3 currentMoveVec = new Vector3();

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private bool isDash = true;
    /// <summary>
    /// �ڒn���Ă��邩
    /// </summary>
    private bool isGround = false;
    /// <summary>
    /// �G�C������
    /// </summary>
    private bool isAiming = false;
    /// <summary>
    /// �G�C�����s���o�C���h���L�����ǂ���
    /// </summary>
    private bool isActivateAimBind = false;
    /// <summary>
    /// ��𒆂�
    /// </summary>
    private bool isAvoiding = false;
    /// <summary>
    /// �ߐڍU������
    /// </summary>
    private bool isMeleeAttacking = false;
    /// <summary>
    /// ���@�U������
    /// </summary>
    private bool isMagicAttacking = false;
    /// <summary>
    /// �s�����o���Ȃ��Ȃ�A�j���[�V�������Đ�����
    /// </summary>
    private bool isPlayingUnableToMoveAnim = false;
    /// <summary>
    /// �A�����鎟�̋ߐڍU�����L���ɂȂ��Ă��邩
    /// </summary>
    private bool isEnableNextMeleeAttack = false;
    /// <summary>
    /// �U�����̉�]���o���邩
    /// </summary>
    private bool canAttackingRotate = false;
    /// <summary>
    /// �ߐڍU���̘A���U����
    /// </summary>
    private int meleeAttackingCount = 0;
    /// <summary>
    /// ���݂̈ړ��X�s�[�h
    /// </summary>
    private float currentMoveSpeed = 0;
    /// <summary>
    /// ���݂̎c�薳�G����
    /// </summary>
    private float currentDamageInterval = 0;
    /// <summary>
    /// ���݂̉�𒆂̎���
    /// </summary>
    private float currentAvoidingTime = 0;
    /// <summary>
    /// ���݂̉���C���^�[�o��
    /// </summary>
    private float currentAvoidInterval = 0;
    /// <summary>
    /// Y���̍ő�㏸�X�s�[�h
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
    /// �v���C���[�̈ړ����X�V����
    /// </summary>
    private void UpdatePlayerMove()
    {
        float speedLimit = 0;
        // TODO: �v���C���[�̓������̂����C������
        // �Ζʂɓ��������Ȃǂ��ď�ɐ�����΂Ȃ��悤��Y�x�N�g����0�ɂ���
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
            // ��]������Ȃ��悤�ɉ�]�x�N�g����0�ɂ���
            playerRb.angularVelocity = Vector3.zero;
        }

        // �X�s�[�h�̐�����ݒ肷��
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
        // �X�s�[�h������葁���ꍇ�͌���������
        if (currentMoveSpeed > speedLimit)
        {
            currentMoveSpeed -= moveAcceleration * Time.deltaTime;
            if (currentMoveSpeed < speedLimit)
            {
                currentMoveSpeed = speedLimit;
            }
        }

        // �G�C�����̓J�����̌����Ă�������Ɍ���
        if (isAiming)
        {
            Vector3 cameraDir = cameraTrans.forward;
            cameraDir.y = 0;
            Quaternion cameraRotaion = Quaternion.LookRotation(cameraDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraRotaion, rotateSpeed * Time.deltaTime);
        }

        // ��]�ł���U�����̏ꍇ�͉�]������
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

        // ���͂��Ȃ��ꍇ�͉�]����������������Ȃ�
        if (inputMoveVec.magnitude == 0 || !IsControlable() || isAttacking())
        {
            SetMoveVelocity(currentMoveVec * currentMoveSpeed);
            return;
        }
        else if (!isAiming)
        {
            // ���͂��������Ƀv���C���[����]������
            RotateTowardsDirection(inputMoveVec);
        }

        // ����������
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
    /// �ڒn���Ă��邩���肷��
    /// </summary>
    private void CheckGround()
    {
        RaycastHit raycastHit;
        float rayRadius = playerCollider.radius;
        float rayDistance = CalcDistanceForFoot();
        // SphereCast�𔭎˂��A�n�ʂɓ��������ꍇ�͐ڒn���Ɣ��肷��
        isGround = Physics.SphereCast(GetCenterPos(), rayRadius, Vector3.down, out raycastHit, rayDistance, LayerMaskUtil.GetLayerMaskGrounds());
    }

    /// <summary>
    /// �W�����v������
    /// </summary>
    private void Jump()
    {
        if (!isGround) return;

        Vector3 playerVec = playerRb.velocity;
        playerVec.y = jumpSpeed;

        playerRb.velocity = playerVec;
    }

    /// <summary>
    /// �ߐڍU��
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
    /// �A������ߐڍU���̎��̍U�����Đ�����
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
    /// ���@�U��
    /// </summary>
    private void MagicAttack()
    {
        // �C���N������Ȃ������疂�@���o���Ȃ�
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
    /// ����s�����s��
    /// </summary>
    private void Avoid()
    {
        // ���ҋ@���Ԃ��c���Ă���ꍇ�͉�������Ȃ�
        if (currentAvoidInterval > 0) return;
        // �󒆂ɂ���ꍇ�͉�������Ȃ�
        if (isAvoiding || !isGround) return;

        // �C���N������Ȃ��������������Ȃ�
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
        // �c���̐������J�n����
        afterImageGenerator.StartGenerate();
        //�v���C���[�̕\��������
        SetEnableRenderers(false);

        isAvoiding = true;
    }

    /// <summary>
    /// �񕜂��s��
    /// </summary>
    public void Heal()
    {
        if (currentHp >= maxHp) return;
        if (!GameSceneUIManager.Instance.InkGauge.IsEnoughInk(healInkCost)) return;

        animator.SetTrigger("Heal");
    }

    /// <summary>
    /// ��𒆂̈ړ�����
    /// </summary>
    private void UpdateAvoidMove()
    {
        currentAvoidingTime = Mathf.Clamp(currentAvoidingTime + Time.deltaTime, 0, avoidMoveTime);
        float currentAvoidSpeed = avoidSpeed * avoidMoveSpeedCurve.Evaluate(currentAvoidingTime / avoidMoveTime);

        SetMoveVelocity(currentMoveVec * currentAvoidSpeed);

        if (currentAvoidingTime >= avoidMoveTime)
        {
            // �v���C���[�̕\�������ǂ�
            SetEnableRenderers(true);
            // �c���̐������~�߂�
            afterImageGenerator.StopGenerate();
            isAvoiding = false;
        }
    }

    /// <summary>
    /// �G�C����Ԃ�؂�ւ���
    /// </summary>
    /// <param name="isAiming"></param>
    private void ChangeAnimManage(bool isAiming)
    {
        if (this.isAiming == isAiming) return;

        GameSceneUIManager.Instance.Reticle.OnAim(isAiming);
        playerCamera.OnAim(isAiming);
        animator.SetBool("Aiming", isAiming);
        // �G�C����ԂɂȂ�������OnAim�g���K�[�𔭉΂���
        if (isAiming)
        {
            animator.SetTrigger("OnAim");
        }

        this.isAiming = isAiming;
    }

    /// <summary>
    /// �ړ��x�N�g����RigidBody��Velocity�ɓK�p����
    /// </summary>
    /// <param name="moveVec"></param>
    private void SetMoveVelocity(Vector3 moveVec)
    {
        moveVec.y = playerRb.velocity.y;
        playerRb.velocity = moveVec;

        currentMoveVec = moveVec.normalized;
    }

    /// <summary>
    /// �J�����̑O����������w�肵�������։�]������(�񎟌�����)
    /// </summary>
    /// <param name="rotateDirection"></param>
    private void RotateTowardsDirection(Vector2 rotateDirection)
    {
        RotateTowardsDirection(ConvertInputVecToMoveVec(rotateDirection));
    }

    /// <summary>
    /// �J�����̑O����������w�肵�������։�]������
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
    /// �J�����̑O�������̃N�I�[�^�j�I�����擾������
    /// </summary>
    /// <returns></returns>
    private Quaternion ConvertCameraForwardToQuaternion()
    {
        Vector3 cameraDirection = cameraTrans.forward;
        cameraDirection.y = 0;

        return Quaternion.LookRotation(cameraDirection);
    }

    /// <summary>
    /// �񎟌����͂��ړ��x�N�g���ɕϊ�����
    /// </summary>
    /// <param name="inputVec"></param>
    /// <returns></returns>
    private Vector3 ConvertInputVecToMoveVec(Vector2 inputVec)
    {
        return Vector3.right * inputVec.x + Vector3.forward * inputVec.y;
    }

    /// <summary>
    /// �v���C���[�̒��S�̍��W���擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPos()
    {
        return transform.position + playerCollider.center;
    }

    /// <summary>
    /// �_���[�W���󂯂����̏���
    /// </summary>
    /// <param name="attackPower"></param>
    public override void TakeDamage(int attackPower)
    {
        // HP���Ȃ��ꍇ�̓_���[�W���󂯂Ȃ�����
        if (isDead) return;
        // ��𒆂̓_���[�W���󂯂Ȃ�����
        if (isAvoiding) return;
        // �Q�[���N���A���̓_���[�W���󂯂Ȃ�����
        if (GameSceneManager.Instance.isGameClear) return;
        if (currentDamageInterval > 0) return;

        base.TakeDamage(attackPower);

        currentDamageInterval = damegeInterval;
        GameSceneUIManager.Instance.PlayerHPGauge.SetFillAmountByHp(maxHp, currentHp);

        animator.SetTrigger("Damage");
        // �G�C����Ԃ͉�������
        ChangeAnimManage(false);
        ResetMeleeAttacking();
        OnAttackColliderDisable();

        AudioManager.Instance.PlayRandomPitchSE("ere_damage");

        isMagicAttacking = false;
    }

    /// <summary>
    /// �|���ꂽ�ۂ̏���
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
    /// ���G���Ԃ��X�V����
    /// </summary>
    public void UpdateDamageInterval()
    {
        currentDamageInterval = Mathf.Max(currentDamageInterval - Time.deltaTime, 0);
    }

    /// <summary>
    /// ������Ԃ��X�V����
    /// </summary>
    public void UpdateAvoidInterval()
    {
        currentAvoidInterval = Mathf.Max(currentAvoidInterval - Time.deltaTime, 0);
    }

    /// <summary>
    /// �v���C���[�̃A�j���[�V�������X�V����
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
        // �X�P�[���͒��a�Ȃ̂Ŕ��a��2�{
        myDrawObject.transform.localScale = Vector3.one * currentDrawRadius * 2;
    }

    /// <summary>
    /// ���삪�\�ȏ�Ԃ��ǂ���
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
    /// �ߐڍU���̏�Ԃ����Z�b�g����
    /// </summary>
    private void ResetMeleeAttacking()
    {
        isMeleeAttacking = false;
        meleeAttackingCount = (int)PlayerMeleeAttackState.NotPlaying;
    }

    /// <summary>
    /// �A�j���[�V�����̃g���K�[�����Z�b�g����
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
    /// �����܂ł̒������v�Z����
    /// </summary>
    /// <returns></returns>
    private float CalcDistanceForFoot()
    {
        // �ڒn��������肳���邽�߂̗]���Ȓ���
        float heightMargin = 0.2f;
        float rayRadius = playerCollider.radius;
        float rayDistance = playerCollider.height / 2 - rayRadius + heightMargin;

        return rayDistance;
    }

    /// <summary>
    /// ������炷
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
    /// �ߐڍU�������L���ɂ��� (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnAttackColliderEneble()
    {
        meleeAttackCollider.EnableCollider();
    }

    /// <summary>
    /// �ߐڍU������𖳌��ɂ��� (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnAttackColliderDisable()
    {
        meleeAttackCollider.DisableCollider();
    }

    /// <summary>
    /// �s���s�\�ɂȂ�A�j���[�V�������Đ����ꂽ�ۂ̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnStartUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = true;
    }

    /// <summary>
    /// �s���s�\�ɂȂ�A�j���[�V�������I�������ۂ̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnFinishUnableToMoveAnim()
    {
        isPlayingUnableToMoveAnim = false;
        ResetAnimTrigger();
    }

    /// <summary>
    /// �񕜖��@���r���������̃C�x���g (�A�j���[�V�����C�x���g�p)
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
        // �G�t�F�N�g��1�b��ɔj������
        Destroy(healEffectObj, 1.0f);
    }

    private void OnReachNextMeleeAttack()
    {
        if (!isEnableNextMeleeAttack) return;

        PlayNextMeleeAttack();
    }

    /// <summary>
    /// �ߐڍU���A�j���[�V�������I���������̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnFinishMeleeAttack()
    {
        ResetMeleeAttacking();
    }

    /// <summary>
    /// ���@�U���A�j���[�V�������I���������̃C�x���g (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnFinishMagicAttack()
    {
        isMagicAttacking = false;
        ChangeAnimManage(isActivateAimBind);
    }

    /// <summary>
    /// �U�����ɉ�]�ł���悤�ɂ��� (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnEnableRotateAttack()
    {
        canAttackingRotate = true;
    }

    /// <summary>
    /// �U�����ɉ�]�ł��Ȃ����� (�A�j���[�V�����C�x���g�p)
    /// </summary>
    private void OnDisableRotateAttack()
    {
        canAttackingRotate = false;
    }

    /// <summary>
    /// ���[�V�������ɑ����n�ʂɂ������̃C�x���g (�A�j���[�V�����C�x���g�p)
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