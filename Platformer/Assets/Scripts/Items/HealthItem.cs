using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField]
    private int healthValue = 1;

    public override void Collect(Collider2D collider)
    {
        AgentManager agent = collider.gameObject.GetComponent<AgentManager>();
        if (agent.HealthManager != null)
        {
            agent.HealthManager.ChangeHealth(healthValue);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
