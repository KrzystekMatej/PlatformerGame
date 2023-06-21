using UnityEngine;

internal interface IHittable
{
    void Hit(GameObject gameObject, Weapon attackingWeapon);
    void Hit(int attackDamage);
}