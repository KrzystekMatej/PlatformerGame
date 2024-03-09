using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class PointItem : Item
{
    [SerializeField]
    private int collectValue = 1;

    public override void Collect(Collider2D collider)
    {
        AgentManager agent = collider.gameObject.GetComponent<AgentManager>();
        if (agent.PointManager != null)
        {
            agent.PointManager.AddPoints(collectValue);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
