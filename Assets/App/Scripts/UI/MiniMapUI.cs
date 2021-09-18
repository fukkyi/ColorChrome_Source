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
    /// プレイヤーアイコンをプレイヤーの向きに合わせて回転させる
    /// </summary>
    private void RotatePlayerIconToPlayer()
    {
        Vector3 playerDirection = playerTrans.forward;
        Vector3 cameraDirection = playerCameraTrans.forward;
        // Y軸は考慮しない
        playerDirection.y = 0;
        cameraDirection.y = 0;
        // プレイヤーの向きがカメラに対してどの方向に向いているか
        Vector3 relatedPlayerDirection = Vector3.Cross(cameraDirection, playerDirection);
        // カメラとプレイヤーの角度の差分を求める
        float iconAngle = Vector3.Angle(cameraDirection, playerDirection);
        // 角度を-180～180に調整する
        iconAngle = relatedPlayerDirection.y < 0 ? iconAngle : -iconAngle;

        playerIcon.localRotation = Quaternion.Euler(0, 0, iconAngle);
    }
}
