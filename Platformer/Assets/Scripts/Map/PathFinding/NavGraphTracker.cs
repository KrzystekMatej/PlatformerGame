using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGraphTracker : MonoBehaviour
{
    [SerializeField]
    private float quantizationUpdateInterval;
    [field: SerializeField]
    public PositionQuantizer Quantizer { get; private set; }
    private AgentManager agent;

    private void Awake()
    {
        agent = GetComponent<AgentManager>();
        if (Quantizer.NavGraph == null) Quantizer.NavGraph = FindObjectOfType<NavGraph>();
    }

    private void Start()
    {
        StartCoroutine(UpdateTracker());
    }

    private IEnumerator UpdateTracker()
    {
        while (true)
        {
            Quantizer.QuantizePosition(agent.CenterPosition);
            yield return new WaitForSeconds(quantizationUpdateInterval);
        }
    }


}
