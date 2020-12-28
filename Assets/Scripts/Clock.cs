using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : ClickableObject
{
    //public AudioSource clickSFX;
    //public ParticleSystem particleFX;

    public override void TriggerUpdate()
    {
        GameManager.Instance.ChargeRewindAbility(10);
    }

    public override void PlayFX(Vector3 position)
    {
        //clickSFX.Play();
        //particleFX.Play();
    }
}
