using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapons/MeleeWeapon")]
public class MeleeWeapon : AgentWeapon
{
    public override void Attack(Agent agent, Vector3 direction, LayerMask hitMask)
    {
        AttackDetector.DetectLayerMask = hitMask;
        Vector3 origin = agent.GetCenterPosition() + direction * (AttackDetector.Size.x / 2);
        int colliderCount = AttackDetector.Detect(origin);


        for (int i = 0; i < colliderCount; i++)
        {
            if (AttackDetector.Colliders[i].gameObject == agent.gameObject) continue;
            IHittable damageable = AttackDetector.Colliders[i].GetComponent<IHittable>();
            if (damageable != null) damageable.Hit(agent.TriggerCollider, this);
        }
    }

    public override bool IsUseable(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected || !IsGroundWeapon;
    }

    

    public override void DrawGizmos(Vector3 origin, Vector3 direction)
    {
        AttackDetector.DrawGizmos(origin + direction * (AttackDetector.Size.x / 2));
    }
}
