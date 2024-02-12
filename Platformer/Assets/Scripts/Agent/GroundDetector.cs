using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    private Collider2D objectCollider;
    [field: SerializeField]
    public LayerMask CollisionLayerMask { get; private set; }

    public bool Detected { get; private set; }
    public RaycastHit2D Hit { get; private set; }

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

    private void Awake()
    {
        objectCollider = objectCollider == null ? GetComponent<Collider2D>() : objectCollider;
    }

    public void Detect()
    {
        RaycastHit2D hit = Physics2D.BoxCast
        (
            objectCollider.bounds.center + new Vector3(boxCastXOffset, boxCastYOffset),
            new Vector2(boxCastWidth, boxCastHeight),
            0,
            Vector2.down,
            0,
            CollisionLayerMask
        );

        
        Detected = (hit.collider != null) && hit.collider.IsTouching(objectCollider);
        Hit = hit;
    }

    public Sound GetGroundSound(SoundActionType actionType)
    {
        if (Hit)
        {
            GroundSoundProvider soundProvider = Hit.collider.GetComponent<GroundSoundProvider>();
            if (soundProvider != null) return soundProvider.GetSound(actionType);
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (objectCollider == null)
            return;
        Gizmos.color = Detected ? gizmoColorDetected : gizmoColorNotDetected;

        Gizmos.DrawWireCube(objectCollider.bounds.center + new Vector3(boxCastXOffset, boxCastYOffset), new Vector3(boxCastWidth, boxCastHeight));
    }
}
