using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkGauge : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImage = null;
    [SerializeField]
    private float maxFillPosY = 0;
    [SerializeField]
    private float minFillPosX = 180;
    /// <summary>
    /// �ő�C���N��
    /// </summary>
    [SerializeField]
    private int maxInk = 1000;

    private int currentInk = 0;

    public bool IsEnoughInk(int value)
    {
        return value <= currentInk;
    }

    /// <summary>
    /// �C���N�ʂ�ǉ�����
    /// </summary>
    /// <param name="value"></param>
    public void AddInk(int value)
    {
        currentInk = Mathf.Clamp(currentInk + value, 0, maxInk);
        SetFillAmountByInk(maxInk, currentInk);
    }

    /// <summary>
    /// �C���N���ő�ɂ���
    /// </summary>
    public void AddMaxInk()
    {
        AddInk(maxInk);
    }

    /// <summary>
    /// �C���N�Q�[�W�̗ʂ��C���N�ʂ���ݒ肷��
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="currentHp"></param>
    public void SetFillAmountByInk(int maxInk, int currentInk)
    {
        float inkRate = (float)currentInk / maxInk;
        // �C���N�̗ʂɂ���ăQ�[�W�̍�����ς���
        gaugeImage.rectTransform.anchoredPosition = Vector3.up * Mathf.Lerp(minFillPosX, maxFillPosY, inkRate);
    }
}
