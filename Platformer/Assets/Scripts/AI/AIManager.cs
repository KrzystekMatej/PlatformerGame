using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    private float aiUpdateInterval = 0.1f;

    public AIInputController InputController { get; private set; }
    public AgentManager Agent { get; private set; }
    public BehaviourTreeRunner TreeRunner { get; private set; }
    public Steering Steering { get; private set; }

    private void Awake()
    {
        InputController = GetComponentInParent<AIInputController>();
        Agent = InputController.GetComponentInChildren<AgentManager>();
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
            TreeRunner.TreeUpdate();
            if (Steering != null) Steering.ApplySteering(Agent, InputController);
            yield return new WaitForSeconds(aiUpdateInterval);
        }
    }
}
