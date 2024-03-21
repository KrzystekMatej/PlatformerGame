using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HittableBlock : MonoBehaviour
{

    public void Hit(GameObject gameObject, AttackingWeapon attackingWeapon)
    {
        GetComponent<Animator>().SetTrigger("Attack");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
    }

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }
}
