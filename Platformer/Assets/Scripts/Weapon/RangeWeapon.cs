using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/RangeWeapon")]
public class RangeWeapon : Weapon
{
    public GameObject RangeWeaponPrefab;
    public float FlySpeed = 1f;
    public bool IsUnstoppable = false;


    public override void Attack(Agent agent, Vector3 direction)
    {
        agent.WeaponManager.SetWeaponVisibility(false);
        GameObject flyingObject = Instantiate(RangeWeaponPrefab, agent.TriggerCollider.bounds.center, Quaternion.identity);
        flyingObject.GetComponent<ThrowableWeapon>().Initialize(this, direction, HitMask, agent);
    }

    public override bool IsUseable(Agent agent)
    {
        return agent.GroundDetector.CollisionDetected || !IsGroundWeapon;
    }

    public override void ShowGizmos(Vector3 origin, Vector3 direction)
    {
        return;
    }
}
