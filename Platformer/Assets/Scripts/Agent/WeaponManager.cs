using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public UnityEvent<Sprite> OnSwap;
    public UnityEvent<AttackingWeapon> OnAdd;

    private List<AttackingWeapon> weapons = new List<AttackingWeapon>();
    private int currentWeapon = 0;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetWeaponVisibility(false);
    }

    public void SetWeaponVisibility(bool isVisible)
    {
        spriteRenderer.enabled = isVisible;
    }

    public bool AddWeaponWithSwap(AttackingWeapon weapon)
    {
        if (AddWeapon(weapon))
        {
            OnAdd?.Invoke(weapon);
            SwapWeaponByIndex(weapons.Count - 1);
            return true;
        }
        return false;
    }

    public bool AddWeapon(AttackingWeapon weapon)
    {
        if (weapons.Any(w => w.WeaponName == weapon.WeaponName)) return false;
        weapons.Add(weapon);
        return true;
    }

    public bool RemoveWeapon(AttackingWeapon weapon)
    {
        return weapons.Remove(weapon);
    }

    public bool RemoveWeaponByName(string weaponName)
    {
        int weaponIndex = weapons.FindIndex(w => w.WeaponName == weaponName);
        if (weaponIndex > 0)
        {
            weapons.RemoveAt(weaponIndex);
            return true;
        }
        return false;
    }

    public bool SwapWeapon()
    {
        if (weapons.Count > 0)
        {
            SwapWeaponByIndex((currentWeapon + 1) % weapons.Count);
            return true;
        }
        return false;
    }

    public bool SwapWeaponByName(string weaponName)
    {
        int weaponIndex = weapons.FindIndex(w => w.WeaponName == weaponName);
        if (weaponIndex > 0)
        {
            SwapWeaponByIndex(weaponIndex);
            return true;
        }
        return false;
    }

    private void SwapWeaponByIndex(int weaponIndex)
    {
        currentWeapon = weaponIndex;
        spriteRenderer.sprite = weapons[currentWeapon].WeaponSprite;
        OnSwap?.Invoke(spriteRenderer.sprite);
    }

    public AttackingWeapon GetWeapon()
    {
        return weapons.Count > 0 ? weapons[currentWeapon] : null;
    }
}

