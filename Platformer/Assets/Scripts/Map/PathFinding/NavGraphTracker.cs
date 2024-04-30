using System.Collections;
using System.Linq;
using UnityEngine;

public class NavGraphTracker : MonoBehaviour
{
    [field: SerializeField]
    public NavGraphNode Current { get; private set; }
    [SerializeField]
    private string navGraphName;
    [SerializeField]
    private float quantizationUpdateInterval;


    public NavGraph NavGraph { get; private set; }
    private AgentManager agent;

    private void Awake()
    {
        agent = GetComponent<AgentManager>();
        NavGraph = FindObjectsOfType<NavGraph>().FirstOrDefault(n => n.name == navGraphName);
    }

    private void OnEnable()
    {
        if (NavGraph) StartCoroutine(UpdateTracker());
    }

    private IEnumerator UpdateTracker()
    {
        yield return null;

        while (true)
        {
            Current = NavGraph.QuantizePosition(agent.PhysicsCenter, Current);
            yield return new WaitForSeconds(quantizationUpdateInterval);
        }
    }
}
