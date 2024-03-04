using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentWeapon : Weapon
{
    [field: SerializeField]
    public OverlapDetector AttackDetector { get; private set; }
    public bool IsGroundWeapon;
    public Sprite WeaponSprite;
    public Sound WeaponSound;

    public virtual void Initialize() { }
    public abstract bool IsUseable(Agent agent);
    public abstract void Attack(Agent agent, Vector2 direction, LayerMask hitMask);

#if UNITY_EDITOR
    public abstract void DrawGizmos(Vector2 origin, Vector2 direction);
#endif
}
