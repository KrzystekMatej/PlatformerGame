using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[System.Serializable]
public class PatrolPlatform : ActionNode
{
    [SerializeField]
    float wallDistance = 1.3f;
    [SerializeField]
    float groundDistance = 1f;

    private string wallCheckRight = "Right";
    private string wallCheckLeft = "Left";
    private string groundCheckRight = "RightDown";
    private string groundCheckLeft = "LeftDown";

    public override void Initialize()
    {
        if (UnityEngine.Random.value > 0.5f)
        {
            context.agent.OrientationController.Flip();
        }
    }

    private float GetCurrentDirection()
    {
        float currentHorizontalInput = context.agent.InputController.InputData.MovementVector.x;
        float currentAgentOrientation = context.agent.OrientationController.CurrentOrientation;
        return currentHorizontalInput != 0 ? currentHorizontalInput : currentAgentOrientation;
    }

    private RaycastHit2D GetHit(float currentDirection, string right, string left)
    {
        return context.vision.GetRaycastHit(currentDirection == 1 ? right : left);
    }

    protected override State OnUpdate()
    {
        float currentDirection = GetCurrentDirection();
        RaycastHit2D wallHit = GetHit(currentDirection, wallCheckRight, wallCheckLeft);
        RaycastHit2D groundHit = GetHit(currentDirection, groundCheckRight, groundCheckLeft);
        bool wall = wallHit.collider != null && wallHit.distance < wallDistance && wallHit.collider.GetComponent<Agent>() == null;
        bool ground = groundHit.collider == null || groundHit.distance > groundDistance;
        if (ground || wall)
        {
            currentDirection = -currentDirection;
        }
        context.inputController.SetMovementVector(new Vector2(currentDirection, 0));
        return State.Success;
    }
}
