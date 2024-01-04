using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeapon", menuName = "Weapons/RangeWeapon")]
public class RangeWeapon : AgentWeapon
{
    public GameObject RangeWeaponPrefab;
    public float FlySpeed = 1f;
    public bool IsUnstoppable = false;


    public override void Attack(Agent agent, Vector3 direction, LayerMask hitMask)
    {
        agent.WeaponManager.SetWeaponVisibility(false);
        GameObject flyingObject = Instantiate(RangeWeaponPrefab, agent.GetCenterPosition(), Quaternion.identity);
        flyingObject.GetComponent<Throwable>().Initialize(AttackRange, direction, FlySpeed);
        flyingObject.GetComponent<DamageDealer>().Initialize(agent.TriggerCollider, this, hitMask);
        flyingObject.GetComponent<Rotator>().Direction = direction;
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
