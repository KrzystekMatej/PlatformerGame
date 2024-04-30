using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    private CastDetector detector;
    [SerializeField]
    private Collider2D physicsCollider;

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

    private void OnValidate()
    {
        if (!detector) return;
        detector.OriginOffset = new Vector2(boxCastXOffset, boxCastYOffset);
        detector.Size = new Vector2(boxCastWidth, boxCastHeight);
    }

#endif

    private void Awake()
    {
        physicsCollider = physicsCollider ? physicsCollider : GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        int detectionCount = detector.Detect(physicsCollider.bounds.center);
        Detected = (detectionCount > 0) && detector.Hits[0].collider.IsTouching(physicsCollider);
        Hit = detector.Hits[0];
    }

    public Sound GetGroundSound(StateType stateType)
    {
        if (Detected)
        {
            GroundSoundProvider soundProvider = Hit.collider.GetComponent<GroundSoundProvider>();
            if (soundProvider) return soundProvider.GetSound(stateType);
        }

        return null;
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (!physicsCollider || !detector) return;
        detector.GizmoColor = Detected ? gizmoColorDetected : gizmoColorNotDetected;
        detector.DrawGizmos(physicsCollider.bounds.center);
    }

#endif
}
