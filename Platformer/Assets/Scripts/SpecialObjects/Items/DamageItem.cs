using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    [SerializeField]
    private Collider2D attacker;
    [SerializeField]
    private Weapon damageWeapon;

    public void Initialize(Sound collectSound, Collider2D attacker, Weapon damageWeapon, LayerMask hitMask)
    {
        this.collectSound = collectSound;
        this.attacker = attacker;
        this.damageWeapon = damageWeapon;
        GetComponent<TriggerDetector>().ChangeTriggerMask(hitMask);
    }

    public override void Collect(Collider2D collider)
    {
        AgentManager agent = collider.GetComponent<AgentManager>();
        if (agent != null)
        {
            agent.Hit(attacker, damageWeapon);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
