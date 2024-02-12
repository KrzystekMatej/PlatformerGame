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
    public override void Attack(Agent agent, Vector2 direction, LayerMask hitMask)
    {
        AttackDetector.DetectLayerMask = hitMask;
        Vector2 origin = agent.CenterPosition + direction * (AttackDetector.Size.x / 2);
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
        return agent.GroundDetector.Detected || !IsGroundWeapon;
    }

    

    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        AttackDetector.DrawGizmos(origin + direction * (AttackDetector.Size.x / 2));
    }
}
