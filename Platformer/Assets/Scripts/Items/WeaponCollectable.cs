using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectable : Collectable
{
    [SerializeField]
    private AgentWeapon weapon;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void Start()
    {
        spriteRenderer.sprite = weapon.WeaponSprite;
    }

    public override void Collect(Collider2D collider)
    {
        Agent agent = collider.gameObject.GetComponent<Agent>();
        if (agent.WeaponManager != null)
        {
            agent.WeaponManager.AddWeapon(weapon);
            agent.AudioFeedback.PlaySpecificSound(collectSound);
        }
    }
}
