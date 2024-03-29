using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    [SerializeField]
    public LayerMask HitMask;

    protected override void HandleEnter()
    {
        agent.WeaponManager.SetWeaponVisibility(true);
        if (agent.GroundDetector != null && agent.GroundDetector.Detected) agent.RigidBody.velocity = Vector3.zero;

        agent.AudioFeedback.PlaySpecificSound(agent.WeaponManager.GetWeapon().WeaponSound);
        agent.WeaponManager.GetWeapon().Attack(agent.TriggerCollider, agent.transform.right * agent.OrientationController.CurrentOrientation, HitMask);
    }

    protected override void HandleUpdate() { }

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
