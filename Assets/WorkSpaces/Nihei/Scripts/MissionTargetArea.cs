using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MissionTargetArea : MonoBehaviour
{
    [SerializeField]
    private MissionName targetMission;

    private void OnTriggerEnter(Collider other)
    {
        // コライダーに当たったらミッションをクリアさせる
        if (other.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        MissionManager.Instance.ComplateMission(targetMission);
    }
}
