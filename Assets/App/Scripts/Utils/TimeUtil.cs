using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtil
{
    /// <summary>
    /// �t���[�����X�V���ꂽ���Ԃ̍������擾����
    /// </summary>
    /// <param name="unScaled"></param>
    /// <returns></returns>
    public static float GetDeltaTime(bool unScaled = false)
    {
        return unScaled ? Time.unscaledDeltaTime : Time.deltaTime;
    }
}
