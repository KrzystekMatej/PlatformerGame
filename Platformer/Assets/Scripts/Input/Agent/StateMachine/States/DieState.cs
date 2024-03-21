using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DieState : State
{
    [SerializeField]
    private float deathDuration = 2f;

    protected override void HandleEnter()
    {
        agent.RigidBody.gravityScale = agent.DefaultData.GravityScale;
        agent.Animator.OnAnimationComplete.AddListener(CompleteTheDeath);
        agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
    }

    private void CompleteTheDeath()
    {
        agent.Animator.OnAnimationComplete.RemoveListener(CompleteTheDeath);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(deathDuration);
        agent.OnDeathComplete?.Invoke();
    }

    public override void HandleUpdate()
    {
        agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
    }
}

[CustomEditor(typeof(DieState), true)]
public class DieStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
