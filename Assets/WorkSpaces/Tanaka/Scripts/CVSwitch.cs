using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVSwitch : MonoBehaviour
{

    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string VoiceName)
    {
        switch (VoiceName)
        {
            case "Ere1_1":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "Ere1_2":
                audioSource.PlayOneShot(audioClips[2]);
                break;
            case "Ere1_3":
                audioSource.PlayOneShot(audioClips[3]);
                break;
            case "Ere1_4":
                audioSource.PlayOneShot(audioClips[4]);
                break;
            case "Ere1_5":
                audioSource.PlayOneShot(audioClips[5]);
                break;
            case "Ere1_6":
                audioSource.PlayOneShot(audioClips[6]);
                break;

            case "Keo1_1":
                audioSource.PlayOneShot(audioClips[7]);
                break;
            case "Keo1_2":
                audioSource.PlayOneShot(audioClips[8]);
                break;
            case "Keo1_3":
                audioSource.PlayOneShot(audioClips[9]);
                break;
            case "Keo1_4":
                audioSource.PlayOneShot(audioClips[10]);
                break;
            case "Keo1_5":
                audioSource.PlayOneShot(audioClips[11]);
                break;
           
        }
    }
}
