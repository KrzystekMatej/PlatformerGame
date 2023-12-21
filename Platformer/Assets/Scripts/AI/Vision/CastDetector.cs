using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastDetector : VisionDetector
{
    [field: SerializeField]
    public Vector2 Size {  get; private set; }
    [field: SerializeField]
    public float Distance { get; private set; }
    [field: SerializeField]
    public Vector2 Direction { get; private set; }
    [field: SerializeField]
    public Vector2 Offset { get; private set; }
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }
    [field: SerializeField]
    public CastType CastType { get; private set; }
    public RaycastHit2D Hit { get; private set; }

    public void Initialize(string detectorName, Color gizmoColor, Vector2 size, float distance, Vector2 direction, Vector2 offset, LayerMask layerMask, CastType castType)
    {
        this.DetectorName = detectorName;
        this.gizmoColor = gizmoColor;
        this.Size = size;
        this.Distance = distance;
        this.Direction = direction.normalized;
        this.Offset = offset;
        this.LayerMask = layerMask;
        this.CastType = castType;
    }

    public override void Detect()
    {
        Vector2 origin = (Vector2)transform.position + Offset;
        switch (CastType)
        {
            case CastType.Ray:
                Hit = Physics2D.Raycast(origin, Direction, Distance, LayerMask);
                return;
            case CastType.Box:
                Hit = Physics2D.BoxCast(origin, Size, 0, Direction, Distance, LayerMask);
                return;
            case CastType.Circle:
                Hit = Physics2D.CircleCast(origin, Size.x, Direction, Distance, LayerMask);
                return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector2 origin = (Vector2)transform.position + Offset;
        switch (CastType)
        {
            case CastType.Ray:
                Vector2 end = origin + Direction * Distance;
                Gizmos.DrawLine(origin, end);
                return;
            case CastType.Box:
                Gizmos.DrawWireCube(origin, Size);
                return;
            case CastType.Circle:
                Gizmos.DrawWireSphere(origin, Size.x);
                return;
        }
    }
}

public enum CastType
{
    Ray,
    Box,
    Circle
}
