using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BackgroundData
{
    public (float startX, float length) CurrentBounds;
    public (float startX, float length) NextBounds;
    public bool IsOnLeft;

    public BackgroundData((float, float) currentBounds, (float, float) nextBounds, bool isOnLeft)
    {
        this.CurrentBounds = currentBounds;
        this.NextBounds = nextBounds;
        this.IsOnLeft = isOnLeft;
    }
}
