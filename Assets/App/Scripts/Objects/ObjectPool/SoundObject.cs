using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : PoolableObject
{
    private static readonly float SpatialBlend2D = 0;
    private static readonly float SpatialBlend3D = 1.0f;

    public static readonly float MaxVolume = 1.0f;
    public static readonly float MinVolume = 0;

    private AudioSource audioSource = null;

    private float currentClipLength = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audioSource.loop)
        {
            UpdateLifeTime();
        }
        else if (audioSource.time >= currentClipLength)
        {
            // ループしない場合は音が最後まで流れたら無効化する
            DisableObject();
        }
    }

    /// <summary>
    /// 音を鳴らす
    /// </summary>
    /// <param name="playClip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    private void PlaySound(AudioClip playClip, float volume = 1.0f, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        audioSource.clip = playClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;

        this.lifeTime = lifeTime;
        currentClipLength = playClip.length;

        audioSource.Play();
    }

    /// <summary>
    /// 3D空間上に音を鳴らす
    /// </summary>
    /// <param name="playClip"></param>
    /// <param name="position"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void Play3DSound(AudioClip playClip, Vector3 position, float volume = 1.0f, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        transform.position = position;
        audioSource.spatialBlend = SpatialBlend3D;

        PlaySound(playClip, volume, pitch, lifeTime, loop);
    }

    /// <summary>
    /// 3D空間関係なく音を鳴らす
    /// </summary>
    /// <param name="playClip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void Play2DSound(AudioClip playClip, float volume = 1.0f, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        transform.position = Vector3.zero;
        audioSource.spatialBlend = SpatialBlend2D;

        PlaySound(playClip, volume, pitch, lifeTime, loop);
    }

    /// <summary>
    /// 2Dサウンドをフェードさせながら鳴らす
    /// </summary>
    /// <param name="playClip"></param>
    /// <param name="volume"></param>
    /// <param name="fadeTime"></param>
    /// <param name="startVolume"></param>
    /// <param name="pitch"></param>
    /// <param name="lifeTime"></param>
    /// <param name="loop"></param>
    public void Play2DSoundWithFade(AudioClip playClip, float volume, float fadeTime, float startVolume = 0, float pitch = 1.0f, float lifeTime = Mathf.Infinity, bool loop = false)
    {
        Play2DSound(playClip, startVolume, pitch, lifeTime, loop);
        StartCoroutine(FadeVolume(startVolume, volume, fadeTime));
    }

    /// <summary>
    /// 音を止める
    /// </summary>
    public void StopSound()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 音をフェードさせながら止める
    /// </summary>
    /// <param name="fadeTime"></param>
    public void StopSound(float fadeTime)
    {
        StartCoroutine(FadeVolume(audioSource.volume, MinVolume, fadeTime, () => { audioSource.Stop(); } ));
    }

    /// <summary>
    /// 音をフェードさせながら止める (コルーチン)
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    public IEnumerator StopSoundWithFade(float fadeTime)
    {
        yield return StartCoroutine(FadeVolume(audioSource.volume, MinVolume, fadeTime, () => { audioSource.Stop(); }));
    }

    /// <summary>
    /// ボリュームを変える
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <param name="volume"></param>
    /// <returns></returns>
    public IEnumerator ChangeVolume(float fadeTime, float volume)
    {
        yield return StartCoroutine(FadeVolume(audioSource.volume, volume, fadeTime));
    }

    /// <summary>
    /// 音量をフェードさせる
    /// </summary>
    /// <param name="beforeValue"></param>
    /// <param name="afterValue"></param>
    /// <param name="fadeTime"></param>
    /// <param name="onFinish"></param>
    /// <returns></returns>
    private IEnumerator FadeVolume(float beforeValue, float afterValue, float fadeTime, Action onFinish = null)
    {
        float currentFadeTime = 0;

        while(currentFadeTime <= fadeTime)
        {
            audioSource.volume = Mathf.Lerp(beforeValue, afterValue, currentFadeTime / fadeTime);
            currentFadeTime += TimeUtil.GetDeltaTime(true);

            yield return null;
        }

        audioSource.volume = afterValue;

        onFinish?.Invoke();
    }

    /// <summary>
    /// ミキサーグループを設定する
    /// </summary>
    /// <param name="mixerGroup"></param>
    public void SetMixer(AudioMixerGroup mixerGroup)
    {
        audioSource.outputAudioMixerGroup = mixerGroup;
    }
}
