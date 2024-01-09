using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargeter : SeekTargeter
{
    [field: SerializeField]
    public float MaxPrediction { get; private set; }

    public override SteeringGoal GetGoal(Agent agent)
    {
        SteeringGoal goal = new SteeringGoal();
        Collider2D target = GetTarget(agent);

        if (target != null)
        {
            float distance = (target.bounds.center - agent.GetCenterPosition()).magnitude;
            float speed = agent.RigidBody.velocity.magnitude;
            float prediction = speed <= distance / MaxPrediction ? MaxPrediction : distance / speed;

            Vector3 futurePosition = agent.GetCenterPosition() + target.GetComponent<Rigidbody>().velocity * prediction;

            goal.Position = !isFleeing ? futurePosition : agent.GetCenterPosition() + (agent.GetCenterPosition() - futurePosition);
        }

        return goal;
    }
}