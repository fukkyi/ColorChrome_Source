using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "AudioResourcesData",
  menuName = "ScriptableObject/AudioResources")
]
public class AudioResources : ScriptableObject
{
    public static string AudioResourceDataPath = "AudioResourcesData";

    public AudioClip[] bgmClips = null;
    public AudioClip[] seClips = null;

    /// <summary>
    /// ���O����BGM�N���b�v���擾����
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip FindBgmByName(string clipName)
    {
        return FindClipByName(clipName, bgmClips);
    }

    /// <summary>
    /// ���O����SE�N���b�v���擾����
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip FindSeByName(string clipName)
    {
        return FindClipByName(clipName, seClips);
    }

    /// <summary>
    /// AudioClip�𖼑O����擾����
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="clips"></param>
    /// <returns></returns>
    private AudioClip FindClipByName(string clipName, AudioClip[] clips)
    {
        if (clips == null) return null;

        AudioClip findedClip = null;
        foreach (AudioClip clip in clips)
        {
            if (clip.name != clipName) continue;

            findedClip = clip;
            break;
        }

        if (findedClip == null)
        {
            Debug.LogWarning("�w�肵�����O��AudioClip��������܂���ł����B " + clipName);
        }

        return findedClip;
    }
}
