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
        AgentManager agent = collider.gameObject.GetComponent<AgentManager>();
        if (agent.WeaponManager != null)
        {
            agent.WeaponManager.AddWeaponWithSwap(weapon);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
