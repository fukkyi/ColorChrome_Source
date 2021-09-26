using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class CV : MonoBehaviour
{

    public AudioClip[] sound;
    AudioSource audioSource;
    //AudioClip audioClip;
    private bool audioStart = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = 0f;
        //audioClip = GetComponent<AudioClip>();
    }

    /// <summary>
    /// ボイスの番号を指定して再生する
    /// </summary>
    /// <param name="soundNum">sound配列の再生したい番号</param>
    public void VoicePlay(int soundNum)
    {
        if (!audioStart) 
        {
            audioSource.clip = sound[soundNum];
            audioSource.Play();
            audioStart = true;
        }
    }
    /// <summary>
    /// クリックするかシナリオ文が流れきったら呼ぶ
    /// </summary>
    public void VoiceReset()
    {
        audioStart = false;
    }

    // Scene1
    public void Ere1_1()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[0]);
        audioSource.clip = sound[0];
        audioSource.Play();
    }
    
    public void Ere1_2()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[1]);
        audioSource.clip = sound[1];
        audioSource.Play();
    }
    
    public void Ere1_3()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[2]);
        audioSource.clip = sound[2];
        audioSource.Play();
    }
    
    public void Ere1_4()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[3]);
        audioSource.clip = sound[3];
        audioSource.Play();
    }
    
    public void Ere1_5()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[4]);
        audioSource.clip = sound[4];
        audioSource.Play();
    }
    
    public void Ere1_6()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[5]);
        audioSource.clip = sound[5];
        audioSource.Play();
    }

    public void Keo1_1()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[6]);
        audioSource.clip = sound[6];
        audioSource.Play();
    }

    public void Keo1_2()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[7]);
        audioSource.clip = sound[7];
        audioSource.Play();
    }

    public void Keo1_3()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[8]);
        audioSource.clip = sound[8];
        audioSource.Play();
    }

    public void Keo1_4()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[9]);
        audioSource.clip = sound[9];
        audioSource.Play();
    }

    public void Keo1_5()
    {
        //audioSource.Stop();
        //audioSource.PlayOneShot(sound[10]);
        audioSource.clip = sound[10];
        audioSource.Play();
    }

    public void teller1_1()
    {
        audioSource.clip = sound[11];
        audioSource.Play();
    }

    public void Teller1_2()
    {
        audioSource.clip = sound[12];
        audioSource.Play();
    }

    public void Teller1_3()
    {
        audioSource.clip = sound[13];
        audioSource.Play();
    }
    public void Teller1_4()
    {
        audioSource.clip = sound[14];
        audioSource.Play();
    }
    public void Teller1_5()
    {
        audioSource.clip = sound[15];
        audioSource.Play();
    }

    public void Teller1_6()
    {
        audioSource.clip = sound[16];
        audioSource.Play();
    }

    public void Teller1_7()
    {
        audioSource.clip = sound[17];
        audioSource.Play();
    }
    public void Teller1_8()
    {
        audioSource.clip = sound[18];
        audioSource.Play();
    }
    public void Teller1_9()
    {
        audioSource.clip = sound[19];
        audioSource.Play();
    }

    // Scene2
    public void Ere2_1()
    {
        audioSource.clip = sound[20];
    }
}
