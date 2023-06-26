using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    private Vector2 attackDirection;

    protected override StateTransition[] GetTransitions()
    {
        return new StateTransition[]
        {
            new AttackToFallTransition(),
            new AttackToIdleTransition(),
            new GenericToDieTransition(),
            new GenericToHurtTransition()
        };
    }

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Attack);
        agent.WeaponManager.SetWeaponVisibility(true);
        attackDirection = agent.transform.right * agent.OrientationController.CurrentOrientation;
        if (agent.GroundDetector.CollisionDetected)
        {
            agent.RigidBody.velocity = Vector3.zero;
        }
        PerformAttack();
    }

    private void PerformAttack()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.WeaponManager.GetWeapon().WeaponSound);
        agent.WeaponManager.GetWeapon().Attack(agent, attackDirection);
    }

    protected override void HandleExit()
    {
        agent.WeaponManager.SetWeaponVisibility(false);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Weapon weapon = agent.WeaponManager.GetWeapon();
        if (weapon != null) weapon.ShowGizmos(agent.TriggerCollider.bounds.center, attackDirection);
    }
}
