using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public UnityEvent<Sprite> OnSwap;
    public UnityEvent<AgentWeapon> OnAdd;

    private List<AgentWeapon> weapons = new List<AgentWeapon>();
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

    public bool AddWeapon(AgentWeapon weapon)
    {
        if (weapons.Any(w => w.name == weapon.name))
            return false;
        weapon.Initialize();
        weapons.Add(weapon);
        currentWeapon = weapons.Count - 1;
        spriteRenderer.sprite = weapons[currentWeapon].WeaponSprite;
        OnSwap?.Invoke(spriteRenderer.sprite);
        OnAdd?.Invoke(weapon);
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

    public AgentWeapon GetWeapon()
    {
        return currentWeapon == -1 ? null : weapons[currentWeapon];
    }
}

