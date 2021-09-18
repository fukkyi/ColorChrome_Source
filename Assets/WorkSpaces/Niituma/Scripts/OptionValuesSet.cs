using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionValuesSet : MonoBehaviour
{
    public void DefaultCameraSpeed()
    {
        OptionValues.cameraSpeed = 0.5f;
        OptionValues.bgmVolume = 1f;
        OptionValues.seVolume = 1f;
    }
    public void SetCameraSpeed(float s)
    {
        OptionValues.cameraSpeed = s;
    }

    public void SetBgmVolume(float v)
    {
        OptionValues.bgmVolume = v;
    }
    public void SetSeVolume(float v)
    {
        OptionValues.seVolume = v;
    }

}