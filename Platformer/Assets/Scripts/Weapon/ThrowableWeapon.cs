using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableWeapon : Throwable
{
    private RangeWeapon rangeWeapon;
    private Agent throwingAgent;

    public void Initialize(RangeWeapon rangeWeapon, Vector2 direction, LayerMask layerMask, Agent agent)
    {
        this.rangeWeapon = rangeWeapon;
        this.flyDistance = rangeWeapon.AttackRange;
        this.direction = direction;
        this.rigidBody.velocity = direction * rangeWeapon.FlySpeed;
        this.throwingAgent = agent;
        GetComponent<TriggerDetector>().ChangeTriggerMask(layerMask);
    }

    public override void PerformHit(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject == throwingAgent.gameObject) return;
            IHittable hittable = collision.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.Hit(throwingAgent.gameObject, rangeWeapon);
                if (!rangeWeapon.IsUnstoppable) Destroy(gameObject);
            }
        }
    }
}
