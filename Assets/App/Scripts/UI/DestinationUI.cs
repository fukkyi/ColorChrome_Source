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
    /// 目標点のアイコンを更新する
    /// </summary>
    private void UpdateDestinationIcon()
    {
        if (destinationTrans == null) return;
        // マップアイコンの位置を更新
        mapIcon.transform.position = destinationTrans.position + Vector3.up * mapIconHeight;

        float canvasScale = transform.root.localScale.z;
        // 画面中心の座標
        Vector3 centerPos = (Vector3.right * Screen.width + Screen.height * Vector3.up) * 0.5f;

        Vector3 iconPos = PlayerCamera.GetCameraComponent().WorldToScreenPoint(destinationTrans.position) - centerPos;
        // 目的地がカメラの後ろにある場合、座標を反転させる
        if (iconPos.z < 0f)
        {
            iconPos.x = -iconPos.x;
            iconPos.y = -iconPos.y;

            if (Mathf.Approximately(iconPos.y, 0f))
            {
                iconPos.y = -centerPos.y;
            }
        }
        // 中心から画面端までの距離を1とした比率
        float iconEdgeRate = Mathf.Max(
            Mathf.Abs(iconPos.x / (centerPos.x - iconEdgeOffset.x)),
            Mathf.Abs(iconPos.y / (centerPos.y - iconEdgeOffset.y))
        );
        // 目的地が画面外にあるか
        bool isOffscreen = (iconPos.z < 0f || iconEdgeRate > 1f);
        if (isOffscreen)
        {
            iconPos.x /= iconEdgeRate;
            iconPos.y /= iconEdgeRate;
            // 矢印アイコンの方向を更新
            arrowRectTrans.eulerAngles = Vector3.forward * (Mathf.Atan2(iconPos.y, iconPos.x) * Mathf.Rad2Deg);
        }
        else
        {
            // 目的地までの距離のテキストを更新
            distanceText.text = ((int)Vector3.Distance(player.transform.position, destinationTrans.position)).ToString() + "m";
        }
        
        myRectTransform.anchoredPosition = iconPos / canvasScale;

        // 画面外の場合は矢印アイコンを出す
        if (arrowRectTrans.gameObject.activeSelf != isOffscreen) arrowRectTrans.gameObject.SetActive(isOffscreen);
        // 画面内の場合は距離テキストを出す
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
