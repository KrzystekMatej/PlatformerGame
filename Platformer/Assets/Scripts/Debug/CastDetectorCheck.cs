using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class CastDetectorCheck : MonoBehaviour
{
    [SerializeField]
    private CastDetector detector;
    [SerializeField]
    private bool isTestingActive;

    private void OnDrawGizmos()
    {
        if (detector == null) return;
        detector.DrawGizmos(transform.position);
        if (isTestingActive)
        {
            int detectionCount = detector.Detect(transform.position);
            string message = "";
            for (int i = 0; i < detectionCount; i++)
            {
                message += $"{detector.Hits[i].collider}";
            }
            if (detectionCount > 0) Debug.Log(message);
        }
    }
}

#endif
