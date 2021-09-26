using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class MissionTargetArea : MonoBehaviour
{
    private UnityEvent onReachedEvent = new UnityEvent();

    private void Awake()
    {
        // ミッションが有効な時だけアクティブにする
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // コライダーに当たったらミッションをクリアさせる
        if (other.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        onReachedEvent.Invoke();
    }

    /// <summary>
    /// 目的地に着いた時のアクションを追加する
    /// </summary>
    /// <param name="action"></param>
    public void AddReachedAction(UnityAction action)
    {
        onReachedEvent.AddListener(action);
    }
}
