using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundKey
{
    public SoundActionType actionType;
    public LayerMask materialLayer;


    public SoundKey(SoundActionType actionType, LayerMask materialLayer)
    {
        this.actionType = actionType;
        this.materialLayer = materialLayer;
    }

    public override bool Equals(object obj)
    {
        if (obj is SoundKey soundKey)
        {
            return soundKey.actionType.Equals(actionType) && soundKey.materialLayer.Equals(materialLayer);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(actionType, materialLayer);
    }
}