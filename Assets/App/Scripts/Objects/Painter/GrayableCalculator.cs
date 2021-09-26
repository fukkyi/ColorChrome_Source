using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayableCalculator : MonoBehaviour
{
    [SerializeField]
    private GrayableObject[] grayableObjects = null;
    [SerializeField]
    private int inkAddRate = 20;

    private int totalGrayAmount = 0;
    private int currentGrayAmount = 0;
    private int currentInkValue = 0;

    public float GrayableRate { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalGrayAmount = ClacTotalGrayAmount();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrayAmount();
    }
    
    /// <summary>
    /// �X�e�[�W�̊D�F�ʂ��X�V����
    /// </summary>
    private void UpdateGrayAmount()
    {
        int beforeGrayAmount = currentGrayAmount;
        int beforeInkValue = currentInkValue;

        currentGrayAmount = ClacCurrentGrayAmount();
        currentInkValue = currentGrayAmount / inkAddRate;

        int diffGrayAmount = currentGrayAmount - beforeGrayAmount;
        int diffInkValue = currentInkValue - beforeInkValue;

        GrayableRate = (float)currentGrayAmount / totalGrayAmount;

        // �C���N�ʂ�ω�������
        GameSceneUIManager.Instance.InkGauge.AddInk(Mathf.Max(diffInkValue, 0));
    }

    /// <summary>
    /// �X�e�[�W�̑��D�F�ʂ��v�Z����
    /// </summary>
    /// <returns></returns>
    private int ClacTotalGrayAmount()
    {
        int grayAmount = 0;
        foreach(GrayableObject grayable in grayableObjects)
        {
            grayAmount += grayable.MaxGrayAmount;
        }

        return grayAmount;
    }

    /// <summary>
    /// ���݂̊D�F�ʂ��v�Z����
    /// </summary>
    /// <returns></returns>
    private int ClacCurrentGrayAmount()
    {
        int grayAmount = 0;
        foreach (GrayableObject grayable in grayableObjects)
        {
            grayAmount += grayable.CurrentGrayAmount;
        }

        return grayAmount;
    }

    /// <summary>
    /// GrayableObjects���q�I�u�W�F�N�g���玩���Őݒ肷��
    /// </summary>
    [ContextMenu("SetGrayableByChilds")]
    private void SetGrayableObjectsByChilds()
    {
        grayableObjects = GetComponentsInChildren<GrayableObject>();
    }
}
