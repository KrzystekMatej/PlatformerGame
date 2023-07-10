using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : Collectable
{
    [SerializeField]
    private int healthValue = 1;

    public override void Collect(Collider2D collider)
    {
        Agent agent = collider.gameObject.GetComponent<Agent>();
        if (agent.HealthManager != null)
        {
            agent.HealthManager.ChangeHealth(healthValue);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
