using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HurtState : State
{
    protected override void HandleEnter() => agent.RigidBody.velocity = Vector2.zero;

    protected override void HandleUpdate() { }

    protected override void HandleExit() { }
}

[CustomEditor(typeof(HurtState), true)]
public class HurtStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

