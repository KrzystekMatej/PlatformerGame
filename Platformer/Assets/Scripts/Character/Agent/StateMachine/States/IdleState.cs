using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class IdleState : State
{
    protected override void HandleEnter() => agent.RigidBody.velocity = Vector2.zero;

    protected override void HandleUpdate() { }

    protected override void HandleExit() { }
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
