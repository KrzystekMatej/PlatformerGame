using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class PointCollectable : Collectable
{
    [SerializeField]
    private int collectValue = 1;

    public override void Collect(Collider2D collider)
    {
        Agent agent = collider.gameObject.GetComponent<Agent>();
        if (agent.PointManager != null)
        {
            agent.PointManager.AddPoints(collectValue);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
