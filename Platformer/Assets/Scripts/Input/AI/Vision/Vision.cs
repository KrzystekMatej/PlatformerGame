using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField]
    private float visionDelay = 0.1f;
    private VisionDetector[] detectors;

    private void Awake()
    {
        detectors = GetComponents<VisionDetector>();
    }

    private void Start()
    {
        foreach (var detector in detectors)
        {
            StartCoroutine(detector.Detect(visionDelay));
        }
    }
}