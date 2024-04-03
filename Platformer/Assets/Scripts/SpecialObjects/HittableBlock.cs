using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HittableBlock : MonoBehaviour, IHittable
{

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }

    public void Hit(Collider2D attacker, Weapon damageWeapon)
    {
        if (damageWeapon is AttackingWeapon attackingWeapon && attackingWeapon.IsGroundWeapon)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<AudioSource>().Play();
        }
    }
}
