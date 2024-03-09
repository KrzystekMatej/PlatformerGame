using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/RangeWeapon")]
public class RangeWeapon : AttackingWeapon
{
    public GameObject DamageItemPrefab;
    public Sound HitSound;
    public float FlySpeed = 1f;
    public bool IsUnstoppable = false;
    public float RotationSpeed = 0;


    public override void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask)
    {
        GameObject damageItem = Instantiate(DamageItemPrefab, attacker.bounds.center, Quaternion.identity);
        damageItem.GetComponent<DamageItem>().Initialize(HitSound, attacker, this, hitMask);
        damageItem.GetComponent<SpriteRenderer>().sprite = WeaponSprite;
        damageItem.AddComponent<Mover>().Initialize(AttackDetector.Size.x, direction * FlySpeed);
        if (RotationSpeed > 0) damageItem.AddComponent<Rotator>().Initialize(RotationSpeed, direction.x);
    }

#if UNITY_EDITOR
    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        return;
    }
#endif
}
