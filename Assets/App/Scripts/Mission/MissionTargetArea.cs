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
        // �~�b�V�������L���Ȏ������A�N�e�B�u�ɂ���
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���C�_�[�ɓ���������~�b�V�������N���A������
        if (other.gameObject.layer != LayerMaskUtil.PlayerLayerNumber) return;

        onReachedEvent.Invoke();
    }

    /// <summary>
    /// �ړI�n�ɒ��������̃A�N�V������ǉ�����
    /// </summary>
    /// <param name="action"></param>
    public void AddReachedAction(UnityAction action)
    {
        onReachedEvent.AddListener(action);
    }
}
