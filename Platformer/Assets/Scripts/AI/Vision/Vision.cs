using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

#if UNITY_EDITOR
public class Vision : MonoBehaviour
{
    [SerializeField]
    private List<VisionDetector> detectors = new List<VisionDetector>();

    private Agent gizmoAgent;

    private void Start()
    {
        gizmoAgent = GetComponentInParent<AIManager>().Agent;
    }

    public void OnDrawGizmosSelected()
    {
        Vector2 origin;
        if (!Application.isPlaying)
        {
            Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
            origin = agent.GetComponent<Collider2D>().bounds.center;
        }
        else origin = gizmoAgent.CenterPosition;

        foreach (VisionDetector detector in detectors)
        {
            detector.DrawGizmos(origin);
        }
    }
}
#endif