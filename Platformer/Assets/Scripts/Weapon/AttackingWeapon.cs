using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AttackingWeapon : Weapon
{
    [field: SerializeField]
    public OverlapDetector AttackDetector { get; private set; }
    public bool IsGroundWeapon;
    public Sprite WeaponSprite;
    public Sound WeaponSound;

    public abstract void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask);

    public int DetectInAttackRange(Vector2 attackerCenter, Vector2 direction, LayerMask hitMask) => DetectInAttackRange(attackerCenter, direction, hitMask, Vector2.zero);

    public int DetectInAttackRange(Vector2 attackerCenter, Vector2 direction, LayerMask hitMask, Vector2 offset)
    {
        AttackDetector.DetectLayerMask = hitMask;
        AttackDetector.OriginOffset = offset + direction * (AttackDetector.Size.x / 2);
        AttackDetector.Angle = MathUtility.GetVectorRadAngle(direction) * Mathf.Rad2Deg;

        int colliderCount = AttackDetector.Detect(attackerCenter);
        return colliderCount;
    }
    public bool IsUseable(AgentManager agent)
    {
        return !IsGroundWeapon || (agent.GroundDetector && agent.GroundDetector.Detected);
    }

#if UNITY_EDITOR

    public void DrawGizmos(Vector2 origin, Vector2 direction) => DrawGizmos(origin, direction, Vector2.zero);

    public void DrawGizmos(Vector2 origin, Vector2 direction, Vector2 offset)
    {
        Vector2 offSet = AttackDetector.OriginOffset;
        AttackDetector.OriginOffset = offset + direction * new Vector2(AttackDetector.Size.x / 2, 0);
        AttackDetector.Angle = MathUtility.GetVectorRadAngle(direction) * Mathf.Rad2Deg;
        AttackDetector.DrawGizmos(origin);

        AttackDetector.OriginOffset = offSet;
    }
#endif
}