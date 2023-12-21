using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField]
    private float visionDelay = 0.1f;
    private Dictionary<string, VisionDetector> detectorTable = new Dictionary<string, VisionDetector>();

    private void Awake()
    {
        foreach (var detector in GetComponents<VisionDetector>())
        {
            detectorTable[detector.DetectorName] = detector;
        }
    }


    private void Start()
    {
        foreach (var detector in detectorTable.Values)
        {
            detector.StartCoroutine(RunDetector(detector));
        }
    }

    public VisionDetector GetDetector(string detectorName)
    {
        return detectorTable[detectorName];
    }

    public void AddCastDetector(string detectorName, Color gizmoColor, Vector2 size, float distance, Vector2 direction, Vector2 offset, LayerMask layerMask, CastType castType)
    {
        CastDetector detector = gameObject.AddComponent<CastDetector>();
        detector.Initialize(detectorName, gizmoColor, size, distance, direction, offset, layerMask, castType);
        detectorTable[detector.DetectorName] = detector;
        detector.StartCoroutine(RunDetector(detector));
    }

    public void AddAreaDetector(string detectorName, Color gizmoColor, float detectionRadius, int maxCollidersToDetect, LayerMask detectLayerMask, LayerMask blockLayerMask)
    {
        AreaDetector detector = gameObject.AddComponent<AreaDetector>();
        detector.Initialize(detectorName, gizmoColor, detectionRadius, maxCollidersToDetect, detectLayerMask, blockLayerMask);
        detectorTable[detector.DetectorName] = detector;
        detector.StartCoroutine(RunDetector(detector));
    }

    public void StartDetector(string detectorName)
    {
        VisionDetector detector = detectorTable[detectorName];
        detector.StartCoroutine(RunDetector(detector));
    }

    public void StopDetector(string detectorName)
    {
        VisionDetector detector = detectorTable[detectorName];
        detector.StopAllCoroutines();
    }

    private IEnumerator RunDetector(VisionDetector detector)
    {
        while (true)
        {
            detector.Detect();
            yield return new WaitForSeconds(visionDelay);
        }
    }
}