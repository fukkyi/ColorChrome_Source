using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FootstepRinger : MonoBehaviour
{
    protected static readonly float FootStepVolume = 0.2f;

    protected static readonly string GrassFootStepSoundName = "Grass footstep 1";
    protected static readonly string GravelFootStepSoundName = "Gravel Footstep 2";
    protected static readonly string StoneFootStepSoundName = "Stone footstep 9";
    protected static readonly string SnowFootStepSoundName = "Ice footstep 1";
    protected static readonly string IceFootStepSoundName = "Ice footstep 1";

    /// <summary>
    /// ë´âπÇñ¬ÇÁÇ∑
    /// </summary>
    public virtual void PlayFootsteps(Vector2 textureCoord, Vector3? playPosition = null) { }

    /// <summary>
    /// ë´âπÇÃâπê∫Çñ¬ÇÁÇ∑
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="position"></param>
    protected void PlayFootstepsSound(FootStepType footStepType, Vector3? position = null)
    {
        string soundName = string.Empty;
        switch (footStepType)
        {
            case FootStepType.Grass:
                soundName = GrassFootStepSoundName;
                break;
            case FootStepType.Gravel:
                soundName = GravelFootStepSoundName;
                break;
            case FootStepType.Stone:
                soundName = StoneFootStepSoundName;
                break;
            case FootStepType.Snow:
                soundName = SnowFootStepSoundName;
                break;
            case FootStepType.Ice:
                soundName = IceFootStepSoundName;
                break;
        }

        if (soundName == string.Empty) return; 

        if (position == null)
        {
            AudioManager.Instance.PlayRandomPitchSE(soundName, null, FootStepVolume);
        }
        else
        {
            AudioManager.Instance.PlayRandomPitchSE(soundName, position, FootStepVolume);
        }
    }

    protected enum FootStepType
    {
        Grass,
        Gravel,
        Snow,
        Stone,
        Ice
    }
}
