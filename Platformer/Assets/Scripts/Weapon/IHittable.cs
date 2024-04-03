using UnityEngine;

public interface IHittable
{
    void Hit(Collider2D attacker, Weapon damageWeapon);
}