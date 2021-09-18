using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFootstepRinger : FootstepRinger
{
    [SerializeField]
    private FootStepType footStepType = FootStepType.Grass;

    public override void PlayFootsteps(Vector2 textureCoord, Vector3? playPosition = null)
    {
        base.PlayFootsteps(textureCoord, playPosition);
        PlayFootstepsSound(footStepType, playPosition);
    }
}
