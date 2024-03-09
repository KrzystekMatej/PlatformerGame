using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackingWeapon : Weapon
{
    [field: SerializeField]
    public OverlapDetector AttackDetector { get; private set; }
    public bool IsGroundWeapon;
    public Sprite WeaponSprite;
    public Sound WeaponSound;

    public bool IsUseable(GroundDetector groundDetector)
    {
        return groundDetector.Detected || !IsGroundWeapon;
    }

    public abstract void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask);

#if UNITY_EDITOR
    public abstract void DrawGizmos(Vector2 origin, Vector2 direction);
#endif
}
