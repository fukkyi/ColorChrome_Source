using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Enemy : Character, IReticleReactable
{
    /// <summary>
    /// �n�`�̐F��ς��鏈����L�������鋗��
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
    /// �_���[�W���󂯂����̏���
    /// </summary>
    /// <param name="attackPower"></param>
    public override void TakeDamage(int attackPower)
    {
        if (!this.enabled) return;
        if (isDead) return;

        base.TakeDamage(attackPower);

        UpdateHpGauge();
        // �x����Ԃɂ��鎞�Ԃ�ݒ肷��
        SetCaution();
        SetFlinch();
        currentDamageEffectTime = damageEffectTime;
    }

    /// <summary>
    /// �|���ꂽ�ۂ̏���
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
    /// �G�C�������킳�������̏���
    /// </summary>
    /// <param name="raycastHit"></param>
    public void OnAimed(RaycastHit raycastHit)
    {
        if (!this.enabled) return;

        SetActiveOutLine(true);
    }

    /// <summary>
    /// �G�C�����O�ꂽ���̏���
    /// </summary>
    public void OnUnAimed()
    {
        SetActiveOutLine(false);
    }

    /// <summary>
    /// �n�`�`��R���|�[�l���g���X�V����
    /// </summary>
    protected void UpdateDrawComponent()
    {
        bool enableDraw = TransformUtil.Calc2DDistance(detectPlayer.transform.position, transform.position) <= drawComponentEnableDistance;

        if (drawComponent == null) return;
        if (drawComponent.enabled == enableDraw) return;

        drawComponent.enabled = enableDraw;
    }

    /// <summary>
    /// �G��HP�Q�[�W���X�V����
    /// </summary>
    protected void UpdateHpGauge()
    {
        if (enemyHpGauge == null) return;

        enemyHpGauge.UpdateGaugeFillAmount(currentHp, maxHp);

        if (isDead) return;

        enemyHpGauge.ShowHp();
    }

    /// <summary>
    /// �x����Ԃ̍X�V���s��
    /// </summary>
    protected void UpdateCautioning()
    {
        currentCautionTime = Mathf.Max(0, currentCautionTime - TimeUtil.GetDeltaTime());
    }

    /// <summary>
    /// �Ђ�ݏ�Ԃ̍X�V���s��
    /// </summary>
    protected void UpdateFlinching()
    {
        currentFlinchTime = Mathf.Max(0, currentFlinchTime - TimeUtil.GetDeltaTime());

        if (animator == null) return;

        animator.SetBool("Flinch", IsFlinching());
    }

    /// <summary>
    /// �x����Ԃ��ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsCautioning()
    {
        return currentCautionTime > 0;
    }

    /// <summary>
    /// �Ђ�ݏ�Ԃ��ǂ���
    /// </summary>
    public bool IsFlinching()
    {
        return currentFlinchTime > 0;
    }

    /// <summary>
    /// ���m��Ԃ���t���[���X�V���̏����𕪂��čs��
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
    /// ���������̍s��
    /// </summary>
    protected virtual void ActionForUndetected()
    {
        if (CheckDetectPlayer())
        {
            ChangeEnemySearchStatus(EnemySearchStatus.detected);
        }
    }

    /// <summary>
    /// �������̍s��
    /// </summary>
    protected virtual void ActionForDetected()
    {
        if (CheckLoseSightPlayer())
        {
            ChangeEnemySearchStatus(EnemySearchStatus.loseSight);
        }
    }

    /// <summary>
    /// �����������̍s��
    /// </summary>
    protected virtual void ActionForLoseSight()
    {
        ChangeEnemySearchStatus(EnemySearchStatus.undetected);
    }

    /// <summary>
    /// �Ђ�ݒ��̍s��
    /// </summary>
    protected virtual void ActionForFlinching()
    {

    }

    /// <summary>
    /// ���m��Ԃ�ύX����
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
    /// ������������ɍs������
    /// </summary>
    protected virtual void OnUndetected() { }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected virtual void OnDetected() { }

    /// <summary>
    /// ������������ɍs������
    /// </summary>
    protected virtual void OnLoseSight() { }

    /// <summary>
    /// �v���C���[�����m���ɂ��邩���肷��
    /// </summary>
    /// <returns></returns>
    protected bool CheckDetectPlayer()
    {
        Vector3 enemyPos = transform.position;
        // �v���C���[�����m����͈͓��ɂ��邩
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
        // �x�����Ȃ�S���ʂ���C�Â��悤�ɂ���
        float cunnretDetectAngle = IsCautioning() ? cautionDetectAngle : detectAngle;
        // �v���C���[�����ʂ��猩�Č��m����p�x�O�Ȃ猟�m���Ȃ�
        if (Vector3.Angle(transform.forward, playerPosDiff) > cunnretDetectAngle) return false;

        if (!igroneObstacle)
        {
            // �v���C���[�ƓG�Ԃɏ�Q��������ꍇ�͌��m���Ȃ�
            if (Physics.Raycast(GetEyePos(), playerPosDiff, playerPosDiff.magnitude, LayerMaskUtil.GetLayerMaskGrounds())) return false;
        }

        detectPlayer = player;

        return true;
    }

    /// <summary>
    /// �v���C���[�������������肷��
    /// </summary>
    /// <returns></returns>
    protected bool CheckLoseSightPlayer()
    {
        if (detectPlayer == null) return false;

        Vector3 enemyPos = transform.position;
        // �v���C���[���������͈͊O�ɂ��邩
        int detectCount = Physics.OverlapSphereNonAlloc(enemyPos, loseSightRadius, searchBuffer, LayerMask.GetMask(LayerMaskUtil.PlayerLayerName));

        if (detectCount <= 0) return true;
        // �v���C���[���܂��͈͓��ɂ���Ȃ��Q���̔�����s��
        Vector3 playerPos = detectPlayer.transform.position;
        Vector3 playerPosDiff = playerPos - enemyPos;

        if (!igroneObstacle)
        {
            // �v���C���[�ƓG�Ԃɏ�Q��������ꍇ�͌��������Ƃ���
            if (Physics.Raycast(GetEyePos(), playerPosDiff, playerPosDiff.magnitude, LayerMaskUtil.GetLayerMaskGrounds())) return true;
        }

        return false;
    }

    /// <summary>
    /// �ڂ̍��W���擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEyePos()
    {
        // �ڂ�Transform���Ȃ��ꍇ�͓G�̍��W��Ԃ�
        if (eyeTrans == null)
        {
            return transform.position;
        }

        return eyeTrans.position;
    }

    /// <summary>
    /// ���m�����v���C���[�Ɍ����ď��X�ɉ�]����
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
    /// ����̍��W�Ɍ����ď��X�ɉ�]������
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="igroneYAxis"></param>
    /// <returns></returns>
    protected Quaternion RotateTowardsToPosition(Vector3 position, float rotateSpeed, bool igroneYAxis = true)
    {
        Vector3 myPos = transform.position;
        // Y���𖳎�����ꍇ��Y���W��0�Ƃ��Čv�Z����
        if (igroneYAxis)
        {
            position.y = 0;
            myPos.y = 0;
        }
        Vector3 rotateDirection = position - myPos;

        return Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotateDirection), rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ���ʂɃv���C���[�����邩
    /// </summary>
    /// <param name="detectAngleLimit"></param>
    /// <returns></returns>
    protected bool IsTherePlayerByforward(float detectAngleLimit)
    {
        if (detectPlayer == null) return false;

        Vector3 playerPos = detectPlayer.transform.position;
        // �v���C���[�����ʂ��猩�ē���̊p�x�������肷��
        return IsTherePositionByforward(playerPos, detectAngleLimit);
    }

    /// <summary>
    /// ����̍��W�����ʂɂ��邩
    /// </summary>
    /// <param name="position"></param>
    /// <param name="detectAngleLimit"></param>
    /// <returns></returns>
    protected bool IsTherePositionByforward(Vector3 position, float detectAngleLimit)
    {
        Vector3 enemyPos = transform.position;
        // Y���W�͍l�����Ȃ�
        position.y = 0;
        enemyPos.y = 0;

        Vector3 playerPosDiff = position - enemyPos;
        // ����̍��W�����ʂ��猩�ē���̊p�x�������肷��
        return Vector3.Angle(transform.forward, playerPosDiff) <= detectAngleLimit;
    }

    /// <summary>
    /// �A�j���[�V�������Đ���������܂Ńt���O�𗧂đ�����
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
    /// �|���ꂽ���̃A�j���[�V�������Đ�����
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

        // �����鎞�Ƀp�[�e�B�N�����o��
        Vector3 particlePos = rootTrans == null ? transform.position : rootTrans.position;
        GameSceneManager.Instance.GetDeadExpParticlePool().GetObject<ParticleObject>().PlayOfPosition(particlePos);
        AudioManager.Instance.PlaySE("Magical Impact 30", rootTrans.position);

        explodeObject?.StartExplode();

        Destroy(gameObject);
    }

    /// <summary>
    /// ���S���̃A�j���[�V�������Đ�����Ă��鎞�ɌĂ΂�鏈��
    /// </summary>
    /// <param name="animState"></param>
    protected void OnPlayingDeadAnim(AnimatorStateInfo animState)
    {
        float deadAnimTime = Mathf.Clamp01(animState.normalizedTime);
        foreach (Renderer renderer in myRenderers)
        {
            //�i�X�ƊD�F�ɂ����Ă���
            Material rendererMaterial = renderer.material;
            if (!rendererMaterial.HasProperty("_Threshold")) continue;

            rendererMaterial.SetFloat("_Threshold", deadAnimTime);
        }
    }

    /// <summary>
    /// �_���[�W���󂯂��ۂ̃G�t�F�N�g���X�V����
    /// </summary>
    protected void UpdateDamageEffect()
    {
        if (currentDamageEffectTime <= 0) return;

        currentDamageEffectTime = Mathf.Clamp(currentDamageEffectTime - Time.deltaTime, 0, damageEffectTime);

        float damageEffectRate = currentDamageEffectTime / damageEffectTime;
        foreach (Renderer renderer in myRenderers)
        {
            // �ԐF�ɂ���
            Material rendererMaterial = renderer.material;
            if (!rendererMaterial.HasProperty("_Color")) continue;

            rendererMaterial.SetColor("_Color", Color.Lerp(Color.white, Color.red, damageEffectRate));
        }
    }

    /// <summary>
    /// �A�E�g���C���̕\����؂�ւ���
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveOutLine(bool active)
    {
        if (outLineTrans == null) return;

        outLineTrans.gameObject.SetActive(active);
    }

    /// <summary>
    /// �x����Ԃɂ���
    /// </summary>
    public void SetCaution()
    {
        currentCautionTime = cautionTime;
    }

    /// <summary>
    /// �Ђ�܂���
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
        /// ������
        /// </summary>
        undetected,
        /// <summary>
        /// ������
        /// </summary>
        detected,
        /// <summary>
        /// ��������
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
