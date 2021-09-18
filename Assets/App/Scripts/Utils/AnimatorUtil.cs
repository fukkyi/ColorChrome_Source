using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimatorUtil : MonoBehaviour
{
    /// <summary>
    /// 指定した名前のステートが終了まで待機する
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animName"></param>
    /// <param name="layerNumber"></param>
    /// <returns></returns>
    public static IEnumerator WaitForAnimByName(Animator animator, string animName, int layerNumber = 0, Action<AnimatorStateInfo> onPlayingDeadAnim = null)
    {
        if (animator == null) yield break;

        // HasExitTimeを考慮して待機するアニメーションに遷移するまで待機する
        while (!animator.GetCurrentAnimatorStateInfo(layerNumber).IsName(animName))
        {
            yield return null;
        }
        // 対象のアニメーションが終了するまで待機する
        while (animator.GetCurrentAnimatorStateInfo(layerNumber).IsName(animName))
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerNumber);
            onPlayingDeadAnim?.Invoke(currentState);
            // 再生時間が1周を超えている場合は待機を終了する
            if (currentState.normalizedTime >= 1.0f) break;

            yield return null;
        }
    }
}
