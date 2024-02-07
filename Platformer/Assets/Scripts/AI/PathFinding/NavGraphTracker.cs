using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGraphTracker : MonoBehaviour
{
    [SerializeField]
    private float quantizationUpdateInterval;
    public NavGraphNode Current { get; private set; }
    private NavGraph navGraph;
    private Agent agent;

    private void Start()
    {
        navGraph = FindObjectOfType<NavGraph>();
        agent = GetComponent<Agent>();
        StartCoroutine(UpdateTracker());
    }

    private IEnumerator UpdateTracker()
    {
        while (true)
        {
            Current = navGraph.QuantizePosition(agent.CenterPosition, Current, Current);
            yield return new WaitForSeconds(quantizationUpdateInterval);
        }
    }


}
