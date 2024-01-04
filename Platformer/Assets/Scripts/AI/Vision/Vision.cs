using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEngine;

public class Vision : MonoBehaviour
{
    private FastAccessList<string, VisionDetector> detectorAccessList = new FastAccessList<string, VisionDetector>();

    private void Awake()
    {
        foreach (var detector in GetComponents<VisionDetector>())
        {
            detectorAccessList[detector.DetectorName] = detector;
        }
    }

    public void DetectorUpdate()
    {
        foreach (var detector in detectorAccessList.Values)
        {
            if (detector.enabled)
            {
                detector.Detect();
            }
        }
    }

    public VisionDetector GetDetector(string detectorName)
    {
        return detectorAccessList[detectorName];
    }

    public void AddCastDetector(string detectorName, Color gizmoColor, Vector2 size, float distance, Vector2 direction, Vector2 offset, LayerMask layerMask, CastType castType)
    {
        CastDetector detector = gameObject.AddComponent<CastDetector>();
        detector.Initialize(detectorName, gizmoColor, size, distance, direction, offset, layerMask, castType);
        detectorAccessList[detector.DetectorName] = detector;
    }

    public void AddAreaDetector(string detectorName, Color gizmoColor, float detectionRadius, int maxCollidersToDetect, LayerMask detectLayerMask, LayerMask blockLayerMask)
    {
        AreaDetector detector = gameObject.AddComponent<AreaDetector>();
        detector.Initialize(detectorName, gizmoColor, detectionRadius, maxCollidersToDetect, detectLayerMask, blockLayerMask);
        detectorAccessList[detector.DetectorName] = detector;
    }

    public void StartDetector(string detectorName)
    {
        VisionDetector detector = detectorAccessList[detectorName];
        detector.enabled = true;
    }

    public void StopDetector(string detectorName)
    {
        VisionDetector detector = detectorAccessList[detectorName];
        detector.enabled = false;
    }
}