using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : Collectable
{
    [SerializeField]
    private Weapon weapon;


    private void Start()
    {
        spriteRenderer.sprite = weapon.WeaponSprite;
    }

    public override void Collect(Collider2D collider)
    {
        WeaponManager weaponManager = collider.gameObject.GetComponentInChildren<WeaponManager>();
        if (weaponManager != null) weaponManager.AddWeapon(weapon);
    }
}
