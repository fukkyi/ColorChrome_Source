using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimatorUtil : MonoBehaviour
{
    /// <summary>
    /// �w�肵�����O�̃X�e�[�g���I���܂őҋ@����
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animName"></param>
    /// <param name="layerNumber"></param>
    /// <returns></returns>
    public static IEnumerator WaitForAnimByName(Animator animator, string animName, int layerNumber = 0, Action<AnimatorStateInfo> onPlayingDeadAnim = null)
    {
        if (animator == null) yield break;

        // HasExitTime���l�����đҋ@����A�j���[�V�����ɑJ�ڂ���܂őҋ@����
        while (!animator.GetCurrentAnimatorStateInfo(layerNumber).IsName(animName))
        {
            yield return null;
        }
        // �Ώۂ̃A�j���[�V�������I������܂őҋ@����
        while (animator.GetCurrentAnimatorStateInfo(layerNumber).IsName(animName))
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerNumber);
            onPlayingDeadAnim?.Invoke(currentState);
            // �Đ����Ԃ�1���𒴂��Ă���ꍇ�͑ҋ@���I������
            if (currentState.normalizedTime >= 1.0f) break;

            yield return null;
        }
    }
}
