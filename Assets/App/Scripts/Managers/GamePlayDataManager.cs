using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePlayDataManager
{
    /// <summary>
    /// ���݂̃`�F�b�N�|�C���g
    /// </summary>
    public static CheckPoint currentCheckPoint = null;
    public static int continueCount = 0;
    public static float grayRate = 0;
    public static float playTime = 0;

    /// <summary>
    /// �v���C�f�[�^�����Z�b�g����
    /// </summary>
    public static void ResetPlayData()
    {
        currentCheckPoint = null;
        continueCount = 0;
        grayRate = 0;
        playTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// �O�b�h�G���f�B���O��������Ԃ�
    /// </summary>
    /// <returns></returns>
    public static bool IsValidHappyEnding()
    {
        return continueCount == 0;
    }
}
