using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HurtState : State
{
    protected override void HandleEnter()
    {
        agent.Animator.PlayByType(AnimationType.Hurt);
    }
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

