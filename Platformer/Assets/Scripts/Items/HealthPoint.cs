using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : Collectable
{
    [SerializeField]
    private int healthValue = 1;

    public override void Collect(Collider2D collider)
    {
        Agent agent = collider.gameObject.GetComponent<Agent>();
        agent.HealthManager.ChangeHealth(healthValue);
        agent.AudioFeedback.PlaySpecificSound(collectSound);
    }
}
