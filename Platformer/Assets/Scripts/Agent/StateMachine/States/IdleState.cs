using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class IdleState : State
{
    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(StateType.Idle);
        agent.RigidBody.velocity = agent.GroundDetector.Detected ? Vector2.zero : agent.RigidBody.velocity;
    }
}

[CustomEditor(typeof(IdleState), true)]
public class IdleStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
