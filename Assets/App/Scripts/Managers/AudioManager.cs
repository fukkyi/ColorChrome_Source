using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : BaseManager<AudioManager>
{
    private static readonly string SeObjectPoolPath = "SEObjectPool";
    private static readonly string BgmObjectPoolPath = "BGMObjectPool";
    private static readonly string AudioMixerPath = "AudioMixer";
    private static readonly string BgmMixerGroupName = "BGM";
    private static readonly string SeMixerGroupName = "SE";

    private AudioMixerGroup bgmMixerGroup = null;
    private AudioMixerGroup seMixerGroup = null;

    private ObjectPool seObjectPool = null;
    private ObjectPool bgmObjectPool = null;
    private AudioResources audioResources = null;
    private SoundObject currentBgmObject = null;

    private const float MinRandomPitchDiff = -0.2f;
    private const float MaxRandomPitchDiff = 0.2f;

    protected override void Init()
    {
        base.Init();

        // TODO: Resources.Loadを使わずにAddressableで取得したい
        audioResources = Resources.Load<AudioResources>(AudioResources.AudioResourceDataPath);
        // ミキサーグループを取得する
        AudioMixer audioMixer = Resources.Load<AudioMixer>(AudioMixerPath);
        foreach(AudioMixerGroup mixerGroup in audioMixer.FindMatchingGroups("Master/"))
        {
            if (mixerGroup.name == BgmMixerGroupName)
            {
                bgmMixerGroup = mixerGroup;
            }
            else if (mixerGroup.name == SeMixerGroupName)
            {
                seMixerGroup = mixerGroup;
            }
        }
        // 音再生用のプールを生成する
        seObjectPool = Instantiate(Resources.Load<GameObject>(SeObjectPoolPath), transform).GetComponent<ObjectPool>();
        bgmObjectPool = Instantiate(Resources.Load<GameObject>(BgmObjectPoolPath), transform).GetComponent<ObjectPool>();
        // 音量のデフォルト値を設定する
        audioMixer.SetFloat("BGM", Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(OptionValues.bgmVolume, 0f, 1f)), -80f, 0f));
    }

    /// <summary>
    /// SEを流す
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void PlaySE(string seName, Vector3? position = null, float volume = 1.0f, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        SoundObject soundObject = seObjectPool.GetObject<SoundObject>();
        soundObject.SetMixer(seMixerGroup);
        AudioClip playClip = audioResources.FindSeByName(seName);

        if (position == null)
        {
            soundObject.Play2DSound(playClip, volume, pitch, lifeTime, loop);
        }
        else
        {
            soundObject.Play3DSound(playClip, (Vector3)position, volume, pitch, lifeTime, loop);
        }
    }

    /// <summary>
    /// ランダムにピッチを変えてSEを流す
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="minRandPitchDiff"></param>
    /// <param name="maxRandPitchDiff"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void PlayRandomPitchSE(string seName, Vector3? position = null, float volume = 1.0f, float pitch = 1.0f, float minRandPitchDiff = MinRandomPitchDiff, float maxRandPitchDiff = MaxRandomPitchDiff, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        float randPitchValue = Random.Range(minRandPitchDiff, maxRandPitchDiff);
        pitch += randPitchValue;

        PlaySE(seName, position, volume, pitch, lifeTime, loop);
    }

    /// <summary>
    /// BGMをフェードさせながら流す
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="fadeTime"></param>
    /// <param name="stopFadeTime"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlayBGMWithFade(string bgmName, float fadeTime = 0, float stopFadeTime = 1.0f, float volume = 1.0f, float pitch = 1.0f)
    {
        StartCoroutine(SwitchToBGM(bgmName, fadeTime, stopFadeTime, volume, pitch));
    }

    /// <summary>
    /// BGMをクロスフェードさせながら流す
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="fadeTime"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlayBGMWithCrossFade(string bgmName, float fadeTime = 0, float volume = 1.0f, float pitch = 1.0f)
    {
        SoundObject soundObject = bgmObjectPool.GetObject<SoundObject>();
        soundObject.SetMixer(bgmMixerGroup);
        AudioClip playClip = audioResources.FindBgmByName(bgmName);

        currentBgmObject?.StopSound(fadeTime);
        soundObject.Play2DSoundWithFade(
            playClip: playClip,
            volume: volume,
            fadeTime: fadeTime,
            pitch: pitch,
            loop: true
        );

        currentBgmObject = soundObject;
    }

    /// <summary>
    /// BGMをフェードさせながら止める
    /// </summary>
    /// <param name="stopFadeTime"></param>
    public void StopCurrentBGMWithFade(float stopFadeTime = 1.0f)
    {
        StartCoroutine(StopBGM(stopFadeTime));
    }
    
    /// <summary>
    /// BGMの音量を変える
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="changeFadeTime"></param>
    public void ChangeVolumeCurrentBGM(float volume, float changeFadeTime = 1.0f)
    {
        StartCoroutine(ChangeBGMVolume(volume, changeFadeTime));
    }

    /// <summary>
    /// BGMをフェードさせて切り替える
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="fadeTime"></param>
    /// <param name="stopFadeTime"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <returns></returns>
    private IEnumerator SwitchToBGM(string bgmName, float fadeTime = 0, float stopFadeTime = 1.0f, float volume = 1.0f, float pitch = 1.0f)
    {
        yield return StartCoroutine(StopBGM(stopFadeTime));

        SoundObject soundObject = bgmObjectPool.GetObject<SoundObject>();
        soundObject.SetMixer(bgmMixerGroup);
        AudioClip playClip = audioResources.FindBgmByName(bgmName);

        soundObject.Play2DSoundWithFade(
            playClip: playClip,
            volume: volume,
            fadeTime: fadeTime,
            pitch: pitch,
            loop: true
        );

        currentBgmObject = soundObject;
    }

    /// <summary>
    /// BGMを止める
    /// </summary>
    /// <param name="stopFadeTime"></param>
    /// <returns></returns>
    private IEnumerator StopBGM(float stopFadeTime)
    {
        if (currentBgmObject == null) yield break;

        yield return StartCoroutine(currentBgmObject.StopSoundWithFade(stopFadeTime));
    }

    /// <summary>
    /// BGMの音量を変える
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="changeFadeTime"></param>
    /// <returns></returns>
    private IEnumerator ChangeBGMVolume(float volume, float changeFadeTime)
    {
        if (currentBgmObject == null) yield break;

        yield return StartCoroutine(currentBgmObject.ChangeVolume(changeFadeTime, volume));
    }
}
