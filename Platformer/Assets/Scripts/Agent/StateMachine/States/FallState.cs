using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class FallState : WalkState
{
    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Fall);
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
    }

    public override void HandleUpdate()
    {
        ControlFall();
        CalculateAcceleration();
        CalculateVelocity();
    }

    private void ControlFall()
    {
        agent.InstanceData.Acceleration.y += agent.DefaultData.FallGravityModifier * Physics2D.gravity.y;
    }

    protected override void HandleExit()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(SoundActionType.Land));
    }
}

[CustomEditor(typeof(FallState), true)]
public class FallStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
