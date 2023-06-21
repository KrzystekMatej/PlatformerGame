using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public UnityEvent<Sprite> OnSwap;

    private List<Weapon> weapons = new List<Weapon>();
    private int currentWeapon = -1;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetWeaponVisibility(false);
    }

    public void SetWeaponVisibility(bool visible)
    {
        spriteRenderer.enabled = visible;
    }

    public bool AddWeapon(Weapon weapon)
    {
        if (weapons.Any(w => w.name == weapon.name))
            return false;
        weapon.Initialize();
        weapons.Add(weapon);
        currentWeapon = weapons.Count - 1;
        spriteRenderer.sprite = weapons[currentWeapon].WeaponSprite;
        OnSwap?.Invoke(spriteRenderer.sprite);
        return true;
    }

    public void SwapWeapon()
    {
        if (currentWeapon == -1)
            return;
        currentWeapon = (currentWeapon + 1) % weapons.Count;
        spriteRenderer.sprite = weapons[currentWeapon].WeaponSprite;
        OnSwap?.Invoke(spriteRenderer.sprite);
    }

    public Weapon GetWeapon()
    {
        return currentWeapon == -1 ? null : weapons[currentWeapon];
    }
}

