using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform trackingTarget = null;

    private Transform playerCamera = null;
    private float cameraHeight = 0;
    private float cameraRotX = 0;

    private void Start()
    {
        cameraHeight = transform.position.y;
        cameraRotX = transform.rotation.eulerAngles.x;

        playerCamera = PlayerCamera.GetPlayerCameraTrans();
    }

    private void Update()
    {
        Vector3 cameraPos = trackingTarget.position;
        cameraPos.y = cameraHeight;

        transform.position = cameraPos;

        Vector3 cameraAngle = playerCamera.rotation.eulerAngles;
        cameraAngle.x = cameraRotX;

        transform.rotation = Quaternion.Euler(cameraAngle);
    }
}
