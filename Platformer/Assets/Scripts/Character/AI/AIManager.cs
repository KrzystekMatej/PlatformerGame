using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float aiUpdateInterval;

    public AgentManager Agent { get; private set; }
    public BehaviourTreeInstance TreeRunner { get; private set; }
    public SteeringController Steering { get; private set; }

    private void Awake()
    {
        Agent = GetComponentInParent<InputController>().GetComponentInChildren<AgentManager>();
        Steering = GetComponentInChildren<SteeringController>();
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
                Steering.ApplySteering();
            }
            yield return new WaitForSeconds(aiUpdateInterval);
        }
    }
}
