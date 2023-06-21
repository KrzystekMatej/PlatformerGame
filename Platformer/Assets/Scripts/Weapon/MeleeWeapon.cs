using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapons/MeleeWeapon")]
public class MeleeWeapon : Weapon
{
    public int MaxNumberOfHits;
    protected RaycastHit2D[] hits;

    public override void Initialize()
    {
        hits = new RaycastHit2D[MaxNumberOfHits];
    }

    public override void Attack(Agent agent, LayerMask hitMask, Vector3 direction)
    {
        int hitCount = Physics2D.RaycastNonAlloc(agent.TriggerCollider.bounds.center, direction, hits, AttackRange, hitMask);
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject == agent.gameObject) continue;
            IHittable damageable = hits[i].collider.GetComponent<IHittable>();
            if (damageable != null) damageable.Hit(agent.gameObject, this);
        }
    }

    public override bool IsUseable(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected || !IsGroundWeapon;
    }

    public override void ShowGizmos(Vector3 origin, Vector3 direction)
    {
        Gizmos.DrawLine(origin, origin + direction * AttackRange);
    }
}
