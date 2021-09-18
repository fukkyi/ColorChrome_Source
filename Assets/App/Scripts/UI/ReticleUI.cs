using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeableUI))]
public class ReticleUI : MonoBehaviour
{
    /// <summary>
    /// �G�C���W�I���擾���邽�߂�Ray�̒���
    /// </summary>
    [SerializeField]
    private float aimRayDistance = 100.0f;
    /// <summary>
    /// �W�I�����Ȃ��������ɖڕW�Ƃ��鋗��
    /// </summary>
    [SerializeField]
    private float noTargetDistance = 20.0f;

    private RectTransform myRectTransform = null;
    private FadeableUI myFadeableUI = null;
    private Camera playerCamera = null;
    private IReticleReactable reactedObject = null;

    private Vector3 reticleRayPos = Vector3.zero;
    private Ray reticleRay = new Ray();

    private bool isAiming = false;
    private int reticleLayerMask = -1;

    private void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
        myFadeableUI = GetComponent<FadeableUI>();
        playerCamera = PlayerCamera.GetCameraComponent();
        reticleLayerMask = LayerMaskUtil.GetLayerMaskGroundsAndEnemy();
    }

    private void Update()
    {
        RayCastWithInteractive();
    }

    public void OnAim(bool isAiming)
    {
        this.isAiming = isAiming;

        if (isAiming)
        {
            myFadeableUI.FadeIn();
        }
        else
        {
            myFadeableUI.FadeOut();
        }
    }

    /// <summary>
    /// Ray���΂��A�Ə��ɔ���������̂ɏ��������s����
    /// </summary>
    public void RayCastWithInteractive()
    {
        IReticleReactable reactable = null;
        reticleRay = GetReticleRay();

        if (isAiming && Physics.Raycast(reticleRay, out RaycastHit raycastHit, aimRayDistance, reticleLayerMask))
        {
            Transform hitTrans = raycastHit.transform;
            reactable = hitTrans.GetComponentInParent<IReticleReactable>();

            // ���e�B�N���ɔ��������I�u�W�F�N�g���O�񔽉����N�������I�u�W�F�N�g�ł���ΏƏ��������s��Ȃ�
            if (CompareReact(reactable)) return;
            
            reactable?.OnAimed(raycastHit);
        }

        if (CompareReact(reactable)) return;

        reactedObject?.OnUnAimed();
        reactedObject = reactable;
    }

    public Vector3 GetReticleRayHitPos()
    {
        reticleRay = GetReticleRay();
        if (Physics.Raycast(reticleRay, out RaycastHit raycastHit, aimRayDistance, reticleLayerMask))
        {
            reticleRayPos = raycastHit.point;
        }
        else
        {
            reticleRayPos = reticleRay.GetPoint(noTargetDistance);
        }

        return reticleRayPos;
    }

    /// <summary>
    /// �Ə���Ray���擾����
    /// </summary>
    /// <returns></returns>
    private Ray GetReticleRay()
    {
        Vector2 reticleScreenPos = myRectTransform.position;

        return RectTransformUtility.ScreenPointToRay(playerCamera, reticleScreenPos);
    }

    /// <summary>
    /// �Ə��ɔ������Ă�����̂��O��Ɠ��ꂩ���肷��
    /// </summary>
    /// <param name="reactable"></param>
    /// <returns></returns>
    private bool CompareReact(IReticleReactable reactable)
    {
        return reactable == reactedObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(reticleRay);
        Gizmos.DrawSphere(reticleRayPos, 0.2f);
    }
}
