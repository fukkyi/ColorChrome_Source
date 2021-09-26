using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public const string playerCameraTag = "PlayerCamera";

    [SerializeField]
    private Transform playerTrans = null;
    [SerializeField]
    private Transform cameraParentTrans = null;
    [SerializeField]
    private Transform cameraTrans = null;
    [SerializeField]
    private Player player = null;

    /// <summary>
    /// �J�����̉�]�X�s�[�h
    /// </summary>
    [SerializeField]
    private Vector2 rotateSpeed = Vector2.one;
    /// <summary>
    /// �J������X���̉�]�͈�
    /// </summary>
    [SerializeField]
    private MinMaxRange angleLimitX = new MinMaxRange(-90, 90);
    /// <summary>
    /// �J������Z���̈ړ��͈�
    /// </summary>
    [SerializeField]
    private MinMaxRange cameraDistanceRange = new MinMaxRange(0, 10);
    /// <summary>
    /// �v���C���[����Q���Ɣ��Ȃ��悤�ɂ��鋅�̏�̗̈�̔��a
    /// </summary>
    [SerializeField]
    private float playerShowRadius = 0.5f;
    /// <summary>
    /// �J����������̕����֑J�ڂ���܂ł̎���
    /// </summary>
    [SerializeField]
    private float transitionTime = 0.1f;
    /// <summary>
    /// �J�������J�ڂ���ۂ̃C�[�W���O
    /// </summary>
    [SerializeField]
    private AnimationCurve transitionEasing = null;
    /// <summary>
    /// �G�C�����̃J�����̍��W
    /// </summary>
    [SerializeField]
    private Transform aimCameraTrans = null;
    /// <summary>
    /// �G�C�����̃J������Z���̈ړ��͈�
    /// </summary>
    [SerializeField]
    private MinMaxRange aimingCameraDistanceRange = new MinMaxRange(0, 10);
    /// <summary>
    /// �G�C�����_�ɑJ�ڂ��鎞��
    /// </summary>
    [SerializeField]
    private float aimingTransitionTime = 0.3f;

    /// <summary>
    /// �v���C���[���瑊�ΓI�ȃJ�����̍��W
    /// </summary>
    private Vector3 relationPos = Vector3.zero;
    /// <summary>
    /// �J�������Z�b�g���Ɍ����������
    /// </summary>
    private Vector3 defaultCameraDirection = Vector3.zero;
    /// <summary>
    /// ���͂���Ă����]����
    /// </summary>
    private Vector2 inputRotateVec = Vector2.zero;
    /// <summary>
    /// �J�������Z�b�g�̑J�ڒ���
    /// </summary>
    private bool isTransition = false;
    /// <summary>
    /// �G�C������
    /// </summary>
    private bool isAiming = false;
    /// <summary>
    /// �G�C�����_�ɑJ�ڂ��Ă��鎞��
    /// </summary>
    private float currentAimingTransitionTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // prefab��̍��W���v���C���[����̑��Έʒu�Ƃ��ĕۑ�����
        relationPos = transform.localPosition - playerTrans.localPosition;
        defaultCameraDirection = transform.forward;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateAimTransition();
        UpdateCameraPosition();
        RotateCamera();
        AdjustCameraDistance();
    }

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        inputRotateVec = InputValueConverter.GetRotateCameraValue(context);
    }

    public void OnCameraReset(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Vector3 cameraResetDeirection = playerTrans.forward;
        cameraResetDeirection.y = defaultCameraDirection.y;

        StartCoroutine(TransitionLookAt(cameraResetDeirection));
    }

    public void OnAim(bool isAiming)
    {
        this.isAiming = isAiming;
    }

    /// <summary>
    /// �v���C���[�J�����̃R���|�[�l���g���擾����
    /// </summary>
    /// <returns></returns>
    public static PlayerCamera GetPlayerCamera()
    {
        return GameObject.FindWithTag(playerCameraTag).GetComponent<PlayerCamera>();
    }

    /// <summary>
    /// �v���C���[�J������Transform���擾����
    /// </summary>
    /// <returns></returns>
    public static Transform GetPlayerCameraTrans()
    {
        return GetPlayerCamera().cameraTrans;
    }

    public static Camera GetCameraComponent()
    {
        return GetPlayerCamera().cameraTrans.GetComponent<Camera>();
    }

    /// <summary>
    /// �J�������v���C���[�̓��͂ɂ���ĉ�]������
    /// </summary>
    public void RotateCamera()
    {
        float rotateAngleX = inputRotateVec.y * rotateSpeed.y * OptionValues.cameraSpeed * Time.deltaTime;
        float rotateAngleY = inputRotateVec.x * rotateSpeed.x * OptionValues.cameraSpeed * Time.deltaTime;

        if (OptionValues.xAxisLeftAndRightReversals)
        {
            rotateAngleX = -rotateAngleX;
        }
        if (OptionValues.yAxisUpsideDown)
        {
            rotateAngleY = -rotateAngleY;
        }

        Vector3 cameraAngles = cameraParentTrans.localEulerAngles;
        cameraAngles.x += rotateAngleX;
        cameraAngles.y += rotateAngleY;

        RotateWithClampAngle(cameraAngles);
    }

    /// <summary>
    /// �p�x�����𒴂��Ȃ��悤�ɃJ��������]������
    /// </summary>
    /// <param name="eulerAngles"></param>
    private void RotateWithClampAngle(Vector3 eulerAngles)
    {
        float aroundDegAngle = TransformUtil.AroundDegAngle;
        float invertBorder = TransformUtil.AroundDegAngle / 2;

        if (eulerAngles.x > angleLimitX.max && eulerAngles.x <= invertBorder)
        {
            eulerAngles.x = angleLimitX.max;
        }
        if (eulerAngles.x < angleLimitX.min + aroundDegAngle && eulerAngles.x >= invertBorder)
        {
            eulerAngles.x = angleLimitX.min + aroundDegAngle;
        }

        eulerAngles.z = 0;

        cameraParentTrans.localRotation = Quaternion.Euler(eulerAngles);
    }

    /// <summary>
    /// ��Q���Ńv���C���[�������Ȃ��Ȃ�Ȃ��悤�ɃJ�����̋����𒲐�����
    /// </summary>
    private void AdjustCameraDistance()
    {
        float aimTransitionAmount = CalcEasingAimTransitionAmount();
        float maxDistance = Mathf.Lerp(cameraDistanceRange.max, aimingCameraDistanceRange.max, aimTransitionAmount);
        float minDistance = Mathf.Lerp(cameraDistanceRange.min, aimingCameraDistanceRange.min, aimTransitionAmount);

        Vector3 playerCenterPos = player.GetCenterPos();
        Vector3 rayDirection = -cameraParentTrans.forward;
        float rayDistance = maxDistance - playerShowRadius;
        float cameraDistance;
        RaycastHit raycastHit;

        // �v���C���[����J�����Ɍ�����Ray�𔭎˂���
        if (Physics.SphereCast(playerCenterPos, playerShowRadius, rayDirection, out raycastHit, rayDistance, LayerMaskUtil.GetLayerMaskGrounds()))
        {
            // Ray����Q�������m�������Q������O�ɃJ�������ړ�������
            cameraDistance = raycastHit.distance + playerShowRadius;
            cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);
        }
        else
        {
            cameraDistance = maxDistance;
        }
        cameraTrans.localPosition = Vector3.back * cameraDistance;
    }

    /// <summary>
    /// �J�����̍��W���X�V����
    /// </summary>
    private void UpdateCameraPosition()
    {
        Vector3 defaultPos = playerTrans.position + relationPos;
        Vector3 aimPos = aimCameraTrans.position;
        Vector3 targetPos = Vector3.Lerp(defaultPos, aimPos, CalcEasingAimTransitionAmount());

        transform.position = targetPos;
    }

    /// <summary>
    /// ����̕����փJ������J�ڂ�����
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator TransitionLookAt(Vector3 direction)
    {
        if (isTransition) yield break;

        isTransition = true;

        Quaternion startRotation = cameraParentTrans.rotation;
        Quaternion forwardRotation = Quaternion.LookRotation(direction, Vector3.up);
        float currentTransitionTime = 0;
        float transitionAmount;

        while (currentTransitionTime < transitionTime)
        {
            currentTransitionTime = Mathf.Clamp(currentTransitionTime + Time.deltaTime, 0, transitionTime);
            transitionAmount = transitionEasing.Evaluate(currentTransitionTime / transitionTime);

            cameraParentTrans.localRotation = Quaternion.Slerp(startRotation, forwardRotation, transitionAmount);

            yield return null;
        }

        isTransition = false;
    }

    /// <summary>
    /// �G�C���̑J�ڏ�Ԃ��X�V����
    /// </summary>
    private void UpdateAimTransition()
    {
        if (isAiming)
        {
            currentAimingTransitionTime += Time.deltaTime;
        }
        else
        {
            currentAimingTransitionTime -= Time.deltaTime;
        }

        currentAimingTransitionTime = Mathf.Clamp(currentAimingTransitionTime, 0, aimingTransitionTime);
    }

    /// <summary>
    /// �G�C���J�ڗp�̃C�[�W���O���ꂽ�J�ڗʂ��v�Z����
    /// </summary>
    /// <returns></returns>
    private float CalcEasingAimTransitionAmount()
    {
        return transitionEasing.Evaluate(currentAimingTransitionTime / aimingTransitionTime);
    }
}
