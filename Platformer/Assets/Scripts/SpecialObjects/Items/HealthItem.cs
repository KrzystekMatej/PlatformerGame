using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField]
    private int healthValue = 1;

    public override void Collect(Collider2D collider)
    {
        HealthManager healthManager = collider.GetComponentInChildren<HealthManager>();
        if (healthManager)
        {
            healthManager.AddHealth(healthValue);
            AudioFeedback audio = collider.GetComponentInChildren<AudioFeedback>();
            if (audio) audio.PlaySpecificSound(collectSound);
        }
    }
}
