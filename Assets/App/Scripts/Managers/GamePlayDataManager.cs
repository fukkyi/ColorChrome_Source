using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePlayDataManager
{
    /// <summary>
    /// 現在のチェックポイント
    /// </summary>
    public static CheckPoint currentCheckPoint = null;
    public static int continueCount = 0;
    public static float grayRate = 0;
    public static float playTime = 0;

    /// <summary>
    /// プレイデータをリセットする
    /// </summary>
    public static void ResetPlayData()
    {
        currentCheckPoint = null;
        continueCount = 0;
        grayRate = 0;
        playTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// グッドエンディングが見れる状態か
    /// </summary>
    /// <returns></returns>
    public static bool IsValidHappyEnding()
    {
        return continueCount == 0;
    }
}
