using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGraphTracker : MonoBehaviour
{
    [field: SerializeField]
    public NavGraph NavGraph { get; private set; }
    [SerializeField]
    private float quantizationUpdateInterval;

    public NavGraphNode Current { get; private set; }
    private AgentManager agent;

    private void Awake()
    {
        agent = GetComponent<AgentManager>();
        NavGraph = NavGraph ? NavGraph : FindObjectOfType<NavGraph>();
    }

    private void Start()
    {
        StartCoroutine(UpdateTracker());
    }

    private IEnumerator UpdateTracker()
    {
        while (true)
        {
            Current = NavGraph.QuantizePosition(agent.CenterPosition, Current);
            yield return new WaitForSeconds(quantizationUpdateInterval);
        }
    }
}
