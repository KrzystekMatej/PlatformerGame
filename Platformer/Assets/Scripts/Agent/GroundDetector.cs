using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    private float detectDelay = 0.02f;
    [SerializeField]
    private Collider2D objectCollider;
    [SerializeField]
    private CastDetector detector;

    public bool Detected { get; private set; }
    public RaycastHit2D Hit { get; private set; }

#if UNITY_EDITOR
    [Header("Gizmo parameters")]
    [Range(-2f, 2f)]
    [SerializeField]
    private float boxCastXOffset = -0.1f;
    [Range(-2f, 2f)]
    [SerializeField]
    private float boxCastYOffset = -0.1f;
    [Range(0f, 2f)]
    [SerializeField]
    private float boxCastWidth = 1f, boxCastHeight = 1f;
    [SerializeField]
    private Color gizmoColorDetected = Color.red, gizmoColorNotDetected = Color.green;
#endif

    private void OnValidate()
    {
        detector.OriginOffset = new Vector2(boxCastXOffset, boxCastYOffset);
        detector.Size = new Vector2(boxCastWidth, boxCastHeight);
    }

    private void Awake()
    {
        objectCollider = objectCollider == null ? GetComponent<Collider2D>() : objectCollider;
        StartCoroutine(Detect());
    }

    private IEnumerator Detect()
    {
        while (true)
        {
            int detectionCount = detector.Detect(objectCollider.bounds.center);

            Detected = (detectionCount > 0) && detector.Hits[0].collider.IsTouching(objectCollider);
            Hit = detector.Hits[0];
            yield return new WaitForSeconds(detectDelay);
        } 
    }

    public Sound GetGroundSound(StateType stateType)
    {
        if (Hit)
        {
            GroundSoundProvider soundProvider = Hit.collider.GetComponent<GroundSoundProvider>();
            if (soundProvider != null) return soundProvider.GetSound(stateType);
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (objectCollider == null) return;
        detector.GizmoColor = Detected ? gizmoColorDetected : gizmoColorNotDetected;
        detector.DrawGizmos(objectCollider.bounds.center);
    }
}
