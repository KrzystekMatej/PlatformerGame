using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private float aiUpdateInterval = 0.1f;

    public AIInputController AIInputController { get; private set; }
    public Agent Agent { get; private set; }
    public Vision Vision { get; private set; }
    public BehaviourTreeRunner TreeRunner { get; private set; }
    public Steering Steering { get; private set; }
    [field: SerializeField]
    public ISteeringBehaviour RootBehaviour { get; private set; }

    private void Awake()
    {
        AIInputController = GetComponentInParent<AIInputController>();
        Agent = AIInputController.GetComponentInChildren<Agent>();
        Vision = GetComponentInChildren<Vision>();
        Steering = GetComponentInChildren<Steering>();
        TreeRunner = GetComponentInChildren<BehaviourTreeRunner>();
    }

    private void Start()
    {
        StartCoroutine(AIUpdate());
    }

    private IEnumerator AIUpdate()
    {
        yield return null;

        while (true)
        {
            if (TreeRunner != null) TreeRunner.TreeUpdate();
            if (RootBehaviour != null) Agent.InstanceData.Acceleration += RootBehaviour.GetSteering(Agent, Vision);
            yield return new WaitForSeconds(aiUpdateInterval);
        }
    }
}
