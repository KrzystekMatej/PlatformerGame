using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPoint : Collectable
{
    [SerializeField]
    private int collectValue = 1;

    public override void Collect(Collider2D collider)
    {
        Agent agent = collider.gameObject.GetComponent<Agent>();
        agent.PointManager.AddPoints(collectValue);
        agent.AudioFeedback.PlaySpecificSound(collectSound);
    }
}
