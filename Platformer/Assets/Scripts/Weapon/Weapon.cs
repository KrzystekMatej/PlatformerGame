using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/BasicWeapon")]
public class Weapon : ScriptableObject
{
    public string WeaponName;
    public int AttackDamage;
    public float KnockbackForce;
}
