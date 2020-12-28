using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : ClickableObject
{
    public override void TriggerUpdate()
    {
        GameManager.Instance.RewindTape(1);
    }

    public override void PlayFX(Vector3 position)
    {
        //GameManager.Instance.CreateBurstEffects(position, 0, 0);
    }
}
