using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SeekTarget : ActionNode
{
    [SerializeField]
    private float turnSpeed = 1f;

    public override void Initialize()
    {
        Vector3 target = (Vector3)blackboard.DataTable["CurrentTarget"];
        context.inputController.SetMovementVector(context.steering.Seek(target));
    }

    protected override State OnUpdate()
    {
        Vector3 target = (Vector3)blackboard.DataTable["CurrentTarget"];
        context.inputController.SetMovementVector(context.inputController.InputData.MovementVector + context.steering.Seek(target) * Time.deltaTime * turnSpeed);
        return State.Success;
    }
}
