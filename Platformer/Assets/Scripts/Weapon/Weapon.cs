using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public bool IsGroundWeapon;
    public string WeaponName;
    public int AttackDamage;
    public float AttackRange;
    public float knockbackForce;
    public Sprite WeaponSprite;
    public Sound WeaponSound;

    public virtual void Initialize() { }
    public abstract bool IsUseable(Agent agent);
    public abstract void Attack(Agent agent, LayerMask hitMask, Vector3 direction);

    public abstract void ShowGizmos(Vector3 origin, Vector3 direction);
}
