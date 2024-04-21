#if UNITY_EDITOR

using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

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
                Gizmos.color = Color.green;
                Vector2 direction = (detector.Hits[i].point - (Vector2)transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detector.Size.x, detector.DetectLayerMask);
                Gizmos.DrawWireSphere(hit.point, 0.2f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(hit.point, hit.point + direction * (detector.Size.x - hit.distance));
            }
            Debug.Log(detectionCount);
        }
    }
}

#endif
