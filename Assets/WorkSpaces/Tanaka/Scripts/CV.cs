using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class CV : MonoBehaviour
{

    public AudioClip[] sound;
    AudioSource audioSource;
    //AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().time = 0f;
        //audioClip = GetComponent<AudioClip>();
    }

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
}
