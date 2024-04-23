using System.Collections;
using System.Linq;
using UnityEngine;

public class NavGraphTracker : MonoBehaviour
{
    [SerializeField]
    private string navGraphName;
    [SerializeField]
    private float quantizationUpdateInterval;


    public NavGraph NavGraph { get; private set; }
    public NavGraphNode Current { get; private set; }
    private AgentManager agent;

    private void Awake()
    {
        agent = GetComponent<AgentManager>();
        NavGraph = FindObjectsOfType<NavGraph>().FirstOrDefault(n => n.name == navGraphName);
    }

    private void OnEnable()
    {
        if (NavGraph != null) StartCoroutine(UpdateTracker());
    }

    private IEnumerator UpdateTracker()
    {
        yield return null;

        while (true)
        {
            Current = NavGraph.QuantizePosition(agent.CenterPosition, Current);
            yield return new WaitForSeconds(quantizationUpdateInterval);
        }
    }
}
