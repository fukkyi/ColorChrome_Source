using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager<AudioManager>
{
    private static readonly string SeObjectPoolPath = "SEObjectPool";
    private static readonly string BgmObjectPoolPath = "BGMObjectPool";

    private ObjectPool seObjectPool = null;
    private ObjectPool bgmObjectPool = null;
    private AudioResources audioResources = null;
    private SoundObject currentBgmObject = null;

    private float minRandomPitchDiff = -0.2f;
    private float maxRandomPitchDiff = 0.2f;

    protected override void Init()
    {
        base.Init();

        // TODO: Resources.Load���g�킸��Addressable�Ŏ擾������
        audioResources = Resources.Load<AudioResources>(AudioResources.AudioResourceDataPath);
        // ���Đ��p�̃v�[���𐶐�����
        seObjectPool = Instantiate(Resources.Load<GameObject>(SeObjectPoolPath), transform).GetComponent<ObjectPool>();
        bgmObjectPool = Instantiate(Resources.Load<GameObject>(BgmObjectPoolPath), transform).GetComponent<ObjectPool>();
    }

    /// <summary>
    /// SE�𗬂�
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
    /// �����_���Ƀs�b�`��ς���SE�𗬂�
    /// </summary>
    /// <param name="seName"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void PlayRandomPitchSE(string seName, Vector3? position = null, float volume = 1.0f, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        float randPitchValue = Random.Range(minRandomPitchDiff, maxRandomPitchDiff);
        pitch += randPitchValue;

        PlaySE(seName, position, volume, pitch, lifeTime, loop);
    }


    /// <summary>
    /// BGM���t�F�[�h�����Ȃ��痬��
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
    /// BGM���N���X�t�F�[�h�����Ȃ��痬��
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="fadeTime"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void PlayBGMWithCrossFade(string bgmName, float fadeTime = 0, float volume = 1.0f, float pitch = 1.0f)
    {
        SoundObject soundObject = bgmObjectPool.GetObject<SoundObject>();
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
    /// BGM���t�F�[�h�����Ȃ���~�߂�
    /// </summary>
    /// <param name="stopFadeTime"></param>
    public void StopCurrentBGMWithFade(float stopFadeTime = 1.0f)
    {
        StartCoroutine(StopBGM(stopFadeTime));
    }

    /// <summary>
    /// BGM���t�F�[�h�����Đ؂�ւ���
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
    /// BGM���~�߂�
    /// </summary>
    /// <param name="stopFadeTime"></param>
    /// <returns></returns>
    private IEnumerator StopBGM(float stopFadeTime)
    {
        if (currentBgmObject == null) yield break;

        yield return StartCoroutine(currentBgmObject.StopSoundWithFade(stopFadeTime));
    }
}
