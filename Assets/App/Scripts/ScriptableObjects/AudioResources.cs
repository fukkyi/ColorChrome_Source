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
    /// 名前からBGMクリップを取得する
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip FindBgmByName(string clipName)
    {
        return FindClipByName(clipName, bgmClips);
    }

    /// <summary>
    /// 名前からSEクリップを取得する
    /// </summary>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public AudioClip FindSeByName(string clipName)
    {
        return FindClipByName(clipName, seClips);
    }

    /// <summary>
    /// AudioClipを名前から取得する
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
            Debug.LogWarning("指定した名前のAudioClipが見つかりませんでした。 " + clipName);
        }

        return findedClip;
    }
}
