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
    /// カメラの回転スピード
    /// </summary>
    [SerializeField]
    private Vector2 rotateSpeed = Vector2.one;
    /// <summary>
    /// カメラのX軸の回転範囲
    /// </summary>
    [SerializeField]
    private MinMaxRange angleLimitX = new MinMaxRange(-90, 90);
    /// <summary>
    /// カメラのZ軸の移動範囲
    /// </summary>
    [SerializeField]
    private MinMaxRange cameraDistanceRange = new MinMaxRange(0, 10);
    /// <summary>
    /// プレイヤーが障害物と被らないようにする球体状の領域の半径
    /// </summary>
    [SerializeField]
    private float playerShowRadius = 0.5f;
    /// <summary>
    /// カメラが特定の方向へ遷移するまでの時間
    /// </summary>
    [SerializeField]
    private float transitionTime = 0.1f;
    /// <summary>
    /// カメラが遷移する際のイージング
    /// </summary>
    [SerializeField]
    private AnimationCurve transitionEasing = null;
    /// <summary>
    /// エイム時のカメラの座標
    /// </summary>
    [SerializeField]
    private Transform aimCameraTrans = null;
    /// <summary>
    /// エイム時のカメラのZ軸の移動範囲
    /// </summary>
    [SerializeField]
    private MinMaxRange aimingCameraDistanceRange = new MinMaxRange(0, 10);
    /// <summary>
    /// エイム視点に遷移する時間
    /// </summary>
    [SerializeField]
    private float aimingTransitionTime = 0.3f;

    /// <summary>
    /// プレイヤーから相対的なカメラの座標
    /// </summary>
    private Vector3 relationPos = Vector3.zero;
    /// <summary>
    /// カメラリセット時に向かせる方向
    /// </summary>
    private Vector3 defaultCameraDirection = Vector3.zero;
    /// <summary>
    /// 入力されている回転方向
    /// </summary>
    private Vector2 inputRotateVec = Vector2.zero;
    /// <summary>
    /// カメラリセットの遷移中か
    /// </summary>
    private bool isTransition = false;
    /// <summary>
    /// エイム中か
    /// </summary>
    private bool isAiming = false;
    /// <summary>
    /// エイム視点に遷移している時間
    /// </summary>
    private float currentAimingTransitionTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // prefab上の座標をプレイヤーからの相対位置として保存する
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
    /// プレイヤーカメラのコンポーネントを取得する
    /// </summary>
    /// <returns></returns>
    public static PlayerCamera GetPlayerCamera()
    {
        return GameObject.FindWithTag(playerCameraTag).GetComponent<PlayerCamera>();
    }

    /// <summary>
    /// プレイヤーカメラのTransformを取得する
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
    /// カメラをプレイヤーの入力によって回転させる
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
    /// 角度制限を超えないようにカメラを回転させる
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
    /// 障害物でプレイヤーが見えなくならないようにカメラの距離を調整する
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

        // プレイヤーからカメラに向けてRayを発射する
        if (Physics.SphereCast(playerCenterPos, playerShowRadius, rayDirection, out raycastHit, rayDistance, LayerMaskUtil.GetLayerMaskGrounds()))
        {
            // Rayが障害物を検知したら障害物より手前にカメラを移動させる
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
    /// カメラの座標を更新する
    /// </summary>
    private void UpdateCameraPosition()
    {
        Vector3 defaultPos = playerTrans.position + relationPos;
        Vector3 aimPos = aimCameraTrans.position;
        Vector3 targetPos = Vector3.Lerp(defaultPos, aimPos, CalcEasingAimTransitionAmount());

        transform.position = targetPos;
    }

    /// <summary>
    /// 特定の方向へカメラを遷移させる
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
    /// エイムの遷移状態を更新する
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
    /// エイム遷移用のイージングされた遷移量を計算する
    /// </summary>
    /// <returns></returns>
    private float CalcEasingAimTransitionAmount()
    {
        return transitionEasing.Evaluate(currentAimingTransitionTime / aimingTransitionTime);
    }
}
