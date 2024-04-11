using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WeaponManager : MonoBehaviour
{
    public UnityEvent<Sprite> OnSwap;
    [SerializeField]
    private List<AttackingWeapon> weapons = new List<AttackingWeapon>();
    private int currentWeapon = 0;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeStartingWeapons();
        SetWeaponVisibility(false);
    }

    private void InitializeStartingWeapons()
    {
        List<AttackingWeapon> startWeapons = weapons
            .GroupBy(w => w.WeaponName)
            .Select(group => group.First())
            .ToList();
        weapons.Clear();
        startWeapons.ForEach(w => weapons.Add(w));
        if (weapons.Count > 0) SwapWeaponByIndex(0);
    }

    public void SetWeaponVisibility(bool isVisible)
    {
        spriteRenderer.enabled = isVisible;
    }

    public bool AddWeaponWithSwap(AttackingWeapon weapon)
    {
        if (AddWeapon(weapon))
        {
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider2D collider = GetComponentInParent<Collider2D>();
        OrientationController orientationController = GetComponentInParent<AgentManager>().GetComponentInChildren<OrientationController>();
        AttackingWeapon weapon = GetWeapon();
        if (weapon != null) weapon.DrawGizmos(collider.bounds.center, orientationController.CurrentOrientation);
    }
#endif
}

