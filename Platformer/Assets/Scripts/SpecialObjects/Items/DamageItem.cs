using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : Item
{
    [SerializeField]
    private Collider2D attacker;
    [SerializeField]
    private Weapon damageWeapon;
    [SerializeField]
    private TriggerFilter filter;

    private void Awake()
    {
        filter = filter ? filter : GetComponent<TriggerFilter>();
    }

    public void Initialize(Sound collectSound, Collider2D attacker, Weapon damageWeapon, LayerMask hitMask)
    {
        this.collectSound = collectSound;
        this.attacker = attacker;
        this.damageWeapon = damageWeapon;
        filter.ChangeTriggerMask(hitMask);
    }

    public override void Collect(Collider2D collider)
    {
        if (collider == attacker) return;
        IHittable hittable = collider.GetComponent<IHittable>();
        if (hittable != null)
        {
            hittable.Hit(attacker, damageWeapon);
            PerformCollectActions(collider);
        }
    }
}
