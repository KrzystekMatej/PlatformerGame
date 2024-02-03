using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
public class Vision : MonoBehaviour
{
    [SerializeField]
    private List<VisionDetector> detectors = new List<VisionDetector>();

    public void OnDrawGizmosSelected()
    {
        Agent agent = GetComponentInParent<AIInputController>().GetComponentInChildren<Agent>();
        Vector2 origin = agent.GetComponent<Collider2D>().bounds.center;

        foreach (VisionDetector detector in detectors)
        {
            detector.DrawGizmos(origin);
        }
    }
}
#endif