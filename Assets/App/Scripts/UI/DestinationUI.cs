using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform arrowRectTrans = null;
    [SerializeField]
    private Image iconImage = null;
    [SerializeField]
    private MapIcon mapIcon = null;
    [SerializeField]
    private Text distanceText = null;
    [SerializeField]
    private float mapIconHeight = 5.0f;
    [SerializeField]
    private Vector2 iconEdgeOffset = new Vector2();

    private Player player = null;
    private Transform destinationTrans = null;
    private RectTransform myRectTransform = null;

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        player = Player.GetPlayer();
    }

    private void LateUpdate()
    {
        UpdateDestinationIcon();
    }

    /// <summary>
    /// �ڕW�_�̃A�C�R�����X�V����
    /// </summary>
    private void UpdateDestinationIcon()
    {
        if (destinationTrans == null) return;
        // �}�b�v�A�C�R���̈ʒu���X�V
        mapIcon.transform.position = destinationTrans.position + Vector3.up * mapIconHeight;

        float canvasScale = transform.root.localScale.z;
        // ��ʒ��S�̍��W
        Vector3 centerPos = (Vector3.right * Screen.width + Screen.height * Vector3.up) * 0.5f;

        Vector3 iconPos = PlayerCamera.GetCameraComponent().WorldToScreenPoint(destinationTrans.position) - centerPos;
        // �ړI�n���J�����̌��ɂ���ꍇ�A���W�𔽓]������
        if (iconPos.z < 0f)
        {
            iconPos.x = -iconPos.x;
            iconPos.y = -iconPos.y;

            if (Mathf.Approximately(iconPos.y, 0f))
            {
                iconPos.y = -centerPos.y;
            }
        }
        // ���S�����ʒ[�܂ł̋�����1�Ƃ����䗦
        float iconEdgeRate = Mathf.Max(
            Mathf.Abs(iconPos.x / (centerPos.x - iconEdgeOffset.x)),
            Mathf.Abs(iconPos.y / (centerPos.y - iconEdgeOffset.y))
        );
        // �ړI�n����ʊO�ɂ��邩
        bool isOffscreen = (iconPos.z < 0f || iconEdgeRate > 1f);
        if (isOffscreen)
        {
            iconPos.x /= iconEdgeRate;
            iconPos.y /= iconEdgeRate;
            // ���A�C�R���̕������X�V
            arrowRectTrans.eulerAngles = Vector3.forward * (Mathf.Atan2(iconPos.y, iconPos.x) * Mathf.Rad2Deg);
        }
        else
        {
            // �ړI�n�܂ł̋����̃e�L�X�g���X�V
            distanceText.text = ((int)Vector3.Distance(player.transform.position, destinationTrans.position)).ToString() + "m";
        }
        
        myRectTransform.anchoredPosition = iconPos / canvasScale;

        // ��ʊO�̏ꍇ�͖��A�C�R�����o��
        if (arrowRectTrans.gameObject.activeSelf != isOffscreen) arrowRectTrans.gameObject.SetActive(isOffscreen);
        // ��ʓ��̏ꍇ�͋����e�L�X�g���o��
        if (distanceText.enabled != !isOffscreen) distanceText.enabled = !isOffscreen;
    }

    public void SetDestination(Transform destination)
    {
        destinationTrans = destination;
        gameObject.SetActive(true);
        mapIcon.gameObject.SetActive(true);
    }

    public void UnSetDestination()
    {
        destinationTrans = null;
        gameObject.SetActive(false);
        mapIcon.gameObject.SetActive(false);
    }
}
