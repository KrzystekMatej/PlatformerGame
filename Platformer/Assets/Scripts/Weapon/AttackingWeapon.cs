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

    public abstract void Attack(Collider2D attacker, LayerMask hitMask, Vector2 direction);

    public bool IsUseableByAgent(AgentManager agent)
    {
        return !IsGroundWeapon || agent.GroundDetector.Detected;
    }

    public int DetectInAttackRange(Collider2D attacker, LayerMask hitMask, Vector2 direction) => DetectInAttackRange(attacker, hitMask, direction, Vector2.zero);

    public int DetectInAttackRange(Collider2D attacker, LayerMask hitMask, Vector2 direction, Vector2 offset)
    {
        AttackDetector.DetectLayerMask = hitMask;
        AttackDetector.OriginOffset = offset + direction * (AttackDetector.Size.x / 2);
        AttackDetector.Angle = MathUtility.GetVectorRadAngle(direction) * Mathf.Rad2Deg;

        int colliderCount = AttackDetector.Detect(attacker.bounds.center);
        return colliderCount;
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