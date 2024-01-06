using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueBehaviour : SeekBehaviour
{
    [field: SerializeField]
    public float MaxPrediction { get; private set; }

    public override Vector2 GetSteering(Agent agent, Vision vision)
    {

        Vector2 direction = target.bounds.center - agent.GetCenterPosition();
        float distance = direction.magnitude;
        float speed = agent.RigidBody.velocity.magnitude;
        float prediction;

        prediction = speed <= distance / MaxPrediction ? MaxPrediction : distance / speed;

        return CalculateSteeringForce(agent, target.bounds.center + target.GetComponent<Rigidbody>().velocity * prediction);
    }
}
