using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class JumpState : WalkState
{

    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(StateType.Jump);
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(StateType.Jump));
        agent.RigidBody.velocity = new Vector2(agent.RigidBody.velocity.x, agent.InstanceData.JumpForce);
    }

    public override void HandleUpdate()
    {
        ControlJumpHeight();
        CalculateAcceleration();
        CalculateVelocity();
    }

    private void ControlJumpHeight()
    {
        if (agent.InputController.InputData.Jump == InputState.Inactive)
        {
            agent.InstanceData.Acceleration.y += agent.DefaultData.JumpGravityModifier * Physics2D.gravity.y;
        }
    }
}

[CustomEditor(typeof(JumpState), true)]
public class JumpStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
