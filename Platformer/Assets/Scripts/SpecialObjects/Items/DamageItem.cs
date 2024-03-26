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
        GetComponent<TriggerFilter>().ChangeTriggerMask(hitMask);
    }

    public override void Collect(Collider2D collider)
    {
        IHittable hittable = collider.GetComponent<IHittable>();
        if (hittable != null)
        {
            hittable.Hit(attacker, damageWeapon);
            AudioFeedback audio = collider.GetComponentInChildren<AudioFeedback>();
            if (audio) audio.PlaySpecificSound(collectSound);
        }
    }
}
