using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentWeapon : Weapon
{
    public bool IsGroundWeapon;
    public float AttackRange;
    public Sprite WeaponSprite;
    public Sound WeaponSound;

    public virtual void Initialize() { }
    public abstract bool IsUseable(Agent agent);
    public abstract void Attack(Agent agent, Vector3 direction, LayerMask hitMask);

    public abstract void ShowGizmos(Vector3 origin, Vector3 direction);
}
