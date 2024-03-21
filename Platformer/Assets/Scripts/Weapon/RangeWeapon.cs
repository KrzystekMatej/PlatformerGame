using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/RangeWeapon")]
public class RangeWeapon : AttackingWeapon
{
    public GameObject DamageItemPrefab;
    public float FlySpeed = 1f;
    public bool IsUnstoppable = false;
    public float RotationSpeed = 0;


    public override void Attack(Collider2D attacker, LayerMask hitMask, Vector2 direction)
    {
        GameObject damageItem = Instantiate(DamageItemPrefab, attacker.bounds.center, Quaternion.identity);
        damageItem.GetComponent<DamageItem>().Initialize(null, attacker, this, hitMask);
        damageItem.GetComponent<SpriteRenderer>().sprite = WeaponSprite;
        Mover mover = damageItem.AddComponent<Mover>();
        mover.Initialize(AttackDetector.Size.x, direction * FlySpeed);
        mover.OnMovementFinished.AddListener(() => Destroy(damageItem));
        if (RotationSpeed > 0) damageItem.AddComponent<Rotator>().Initialize(RotationSpeed, direction.x);
    }
}
