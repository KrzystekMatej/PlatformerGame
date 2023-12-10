using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapons/MeleeWeapon")]
public class MeleeWeapon : AgentWeapon
{
    public int MaxNumberOfHits;
    public float AttackWidth = 1;

    protected Collider2D[] colliders;

    public override void Initialize()
    {
        colliders = new Collider2D[MaxNumberOfHits];
    }

    public override void Attack(Agent agent, Vector3 direction, LayerMask hitMask)
    {
        Vector3 origin = agent.TriggerCollider.bounds.center + direction * (AttackRange / 2);
        int colliderCount = Physics2D.OverlapBoxNonAlloc(origin, new Vector2(AttackRange, AttackWidth), 0, colliders, hitMask);


        for (int i = 0; i < colliderCount; i++)
        {
            if (colliders[i].gameObject == agent.gameObject) continue;
            IHittable damageable = colliders[i].GetComponent<IHittable>();
            if (damageable != null) damageable.Hit(agent.gameObject, this);
        }
    }

    public override bool IsUseable(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected || !IsGroundWeapon;
    }

    

    public override void ShowGizmos(Vector3 origin, Vector3 direction)
    {
        Gizmos.DrawWireCube(origin + direction * (AttackRange / 2), new Vector2(AttackRange, AttackWidth));
    }
}
