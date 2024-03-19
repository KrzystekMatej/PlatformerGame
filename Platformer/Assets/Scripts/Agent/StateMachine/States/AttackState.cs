using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    private Vector2 attackDirection;
    [SerializeField]
    public LayerMask HitMask;

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(StateType.Attack);
        agent.WeaponManager.SetWeaponVisibility(true);
        if (agent.GroundDetector != null && agent.GroundDetector.Detected) agent.RigidBody.velocity = Vector3.zero;
        PerformAttack();
    }

    private void PerformAttack()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.WeaponManager.GetWeapon().WeaponSound);
        attackDirection = agent.transform.right * agent.OrientationController.CurrentOrientation;
        agent.WeaponManager.GetWeapon().Attack(agent.TriggerCollider, HitMask, attackDirection);
    }

    protected override void HandleExit()
    {
        agent.WeaponManager.SetWeaponVisibility(false);
    }
}

[CustomEditor(typeof(AttackState), true)]
public class AttackStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

