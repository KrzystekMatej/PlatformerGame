using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

public class WalkState : State
{

    protected override void HandleEnter()
    {
        agent.Animator.OnAnimationAction.AddListener(PlayStepSound);
    }

    private void PlayStepSound()
    {
        agent.AudioFeedback.PlaySpecificSound(agent.GroundDetector.GetGroundSound(StateType.Walk));
    }

    public override void HandleUpdate()
    {
        CalculateAcceleration();
        CalculateVelocity();
    }


    protected virtual void CalculateAcceleration()
    {
        Vector2 inputForce = agent.InputController.InputData.SteeringForce;
        Vector2 decelerationForce = -MathUtility.GetSignedVector(agent.RigidBody.velocity) * agent.InstanceData.MaxForce;

        agent.InstanceData.Acceleration += new Vector2(agent.InputController.DecelerationFlags.x ? decelerationForce.x : inputForce.x, 0);
    }

    protected virtual void CalculateVelocity()
    {
        Vector2 previousVelocity = agent.RigidBody.velocity;

        agent.RigidBody.velocity += agent.InstanceData.Acceleration * Time.deltaTime;

        if (agent.InputController.DecelerationFlags.x && ShouldDecelerationStop(agent.RigidBody.velocity.x, previousVelocity.x))
        {
            agent.RigidBody.velocity = new Vector2(0, agent.RigidBody.velocity.y);
            agent.InputController.DecelerationFlags = (false, false);
        }

        float clampedX = Mathf.Clamp(agent.RigidBody.velocity.x, -agent.InstanceData.MaxSpeed, agent.InstanceData.MaxSpeed);
        agent.RigidBody.velocity = new Vector2(clampedX, agent.RigidBody.velocity.y);
        agent.InstanceData.Acceleration.Set(0, 0);
    }

    protected static bool ShouldDecelerationStop(float currentVelocityComponent, float previousVelocityComponent)
    {
        return currentVelocityComponent == 0 || Math.Sign(currentVelocityComponent) != Math.Sign(previousVelocityComponent);
    }

    protected override void HandleExit()
    {
        agent.Animator.OnAnimationAction.RemoveListener(PlayStepSound);
    }
}


[CustomEditor(typeof(WalkState), true)]
public class WalkStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}
