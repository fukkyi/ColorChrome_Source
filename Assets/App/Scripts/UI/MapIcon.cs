using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    [SerializeField]
    private bool isFixedRotate = false;

    private Transform playerCamera = null;
    private Vector3 iconAngle = new Vector3();

    private void Start()
    {
        iconAngle = transform.rotation.eulerAngles;
        playerCamera = PlayerCamera.GetPlayerCameraTrans();
    }

    void Update()
    {
        RotateIconForFixed();
    }

    /// <summary>
    /// ミニマップに角度を固定するためにアイコンを回転させる
    /// </summary>
    private void RotateIconForFixed()
    {
        if (!isFixedRotate) return;

        Vector3 cameraAngle = playerCamera.rotation.eulerAngles;
        Vector3 iconRotateAngle = iconAngle;
        iconRotateAngle.z += cameraAngle.y;

        transform.rotation = Quaternion.Euler(iconRotateAngle);
    }
}
