using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform playerIcon = null;
    [SerializeField]
    private Transform playerTrans = null;
    [SerializeField]
    private Transform playerCameraTrans = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayerIconToPlayer();
    }

    /// <summary>
    /// �v���C���[�A�C�R�����v���C���[�̌����ɍ��킹�ĉ�]������
    /// </summary>
    private void RotatePlayerIconToPlayer()
    {
        Vector3 playerDirection = playerTrans.forward;
        Vector3 cameraDirection = playerCameraTrans.forward;
        // Y���͍l�����Ȃ�
        playerDirection.y = 0;
        cameraDirection.y = 0;
        // �v���C���[�̌������J�����ɑ΂��Ăǂ̕����Ɍ����Ă��邩
        Vector3 relatedPlayerDirection = Vector3.Cross(cameraDirection, playerDirection);
        // �J�����ƃv���C���[�̊p�x�̍��������߂�
        float iconAngle = Vector3.Angle(cameraDirection, playerDirection);
        // �p�x��-180�`180�ɒ�������
        iconAngle = relatedPlayerDirection.y < 0 ? iconAngle : -iconAngle;

        playerIcon.localRotation = Quaternion.Euler(0, 0, iconAngle);
    }
}
