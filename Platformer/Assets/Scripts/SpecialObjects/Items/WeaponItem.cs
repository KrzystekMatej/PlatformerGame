using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    [SerializeField]
    private AttackingWeapon weapon;


    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = weapon.WeaponSprite;
    }

    public override void Collect(Collider2D collider)
    {
        WeaponManager weaponManager = collider.GetComponentInChildren<WeaponManager>();
        if (weaponManager)
        {
            weaponManager.AddWeaponWithSwap(weapon);
            PerformCollectActions(collider);
        }
    }
}
