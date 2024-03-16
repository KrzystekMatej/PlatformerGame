using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float aiUpdateInterval;

    public AIInputController InputController { get; private set; }
    public AgentManager Agent { get; private set; }
    public BehaviourTreeInstance TreeRunner { get; private set; }
    public Steering Steering { get; private set; }

    private void Awake()
    {
        InputController = GetComponentInParent<AIInputController>();
        Agent = InputController.GetComponentInChildren<AgentManager>();
        Steering = GetComponentInChildren<Steering>();
        TreeRunner = GetComponentInChildren<BehaviourTreeInstance>();
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
            if (Time.timeScale > 0)
            {
                TreeRunner.TreeUpdate();
                if (Steering != null) Steering.ApplySteering(Agent, InputController);
            }
            yield return new WaitForSeconds(aiUpdateInterval);
        }
    }
}
