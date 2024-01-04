using UnityEngine;

internal interface IHittable
{
    void Hit(Collider2D attackerCollider, Weapon attackingWeapon);
    void Hit(int attackDamage);
}