using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtil
{
    /// <summary>
    /// フレームが更新された時間の差分を取得する
    /// </summary>
    /// <param name="unScaled"></param>
    /// <returns></returns>
    public static float GetDeltaTime(bool unScaled = false)
    {
        return unScaled ? Time.unscaledDeltaTime : Time.deltaTime;
    }
}
