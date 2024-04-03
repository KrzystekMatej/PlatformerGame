using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

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
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(detector.Hits[i].point, 0.2f);
                Gizmos.color = Color.yellow;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, detector.Hits[i].point - (Vector2)transform.position, detector.Size.x, detector.DetectLayerMask);
                Debug.Log(hit.collider);
                Gizmos.DrawLine(hit.point, transform.position);
            }
        }
    }
}

#endif
