using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/RangeWeapon")]
public class RangeWeapon : AttackingWeapon
{
    public GameObject DamageItemPrefab;
    public float FlySpeed = 1f;
    public float RotationSpeed = 0;


    public override void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask)
    {
        GameObject damageItem = Instantiate(DamageItemPrefab, attacker.bounds.center, Quaternion.identity);
        damageItem.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * MathUtility.GetVectorRadAngle(direction));
        damageItem.GetComponent<DamageItem>().Initialize(null, attacker, this, hitMask);
        damageItem.GetComponent<SpriteRenderer>().sprite = WeaponSprite;
        Mover mover = damageItem.AddComponent<Mover>();
        mover.Initialize(AttackDetector.Size.x, direction * FlySpeed);
        if (RotationSpeed > 0) damageItem.AddComponent<Rotator>().Initialize(RotationSpeed * direction.x);

        mover.OnMovementFinished.AddListener(() => Destroy(damageItem));
    }
}
