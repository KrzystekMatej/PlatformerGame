using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Search.SearchColumn;

public class CastDetector : VisionDetector
{
    [SerializeField]
    private SerializableDictionary<string, VisionCast> rayTable = new SerializableDictionary<string, VisionCast>();

    private void Awake()
    {
        foreach (var visionCastEntry in rayTable.GetAllStartEntries())
        {
            visionCastEntry.Value.direction = visionCastEntry.Value.direction.normalized;
        }
    }

    public override IEnumerator Detect(float delay)
    {
        while (true)
        {

            foreach (var visionCastEntry in rayTable.GetAllStartEntries())
            {
                PerformCast(visionCastEntry.Value);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    private void PerformCast(VisionCast visionCast)
    {
        Vector2 origin = (Vector2)transform.position + visionCast.offset;
        switch (visionCast.castType)
        {
            case CastType.Ray:
                visionCast.hit = Physics2D.Raycast(origin, visionCast.direction, visionCast.distance, visionCast.layerMask);
                return;
            case CastType.Box:
                visionCast.hit = Physics2D.BoxCast(origin, visionCast.size, 0, visionCast.direction, visionCast.distance, visionCast.layerMask);
                return;
            case CastType.Circle:
                visionCast.hit = Physics2D.CircleCast(origin, visionCast.size.x, visionCast.direction, visionCast.distance, visionCast.layerMask);
                return;
        }
    }

    public VisionCast GetVisionRay(string rayName)
    {
        return rayTable[rayName];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        foreach (var rayEntry in rayTable.GetAllStartEntries())
        {
            DrawCastGizmo(rayEntry.Value);
        }
    }

    private void DrawCastGizmo(VisionCast visionCast)
    {
        Vector2 normalizedDirection = visionCast.direction.normalized;
        Vector2 origin = (Vector2)transform.position + visionCast.offset;
        switch (visionCast.castType)
        {
            case CastType.Ray:
                Vector2 end = origin + normalizedDirection * visionCast.distance;
                Gizmos.DrawLine(origin, end);
                return;
            case CastType.Box:
                Gizmos.DrawWireCube(origin, visionCast.size);
                return;
            case CastType.Circle:
                Gizmos.DrawWireSphere(origin, visionCast.size.x);
                return;
        }
    }

    public void AddWeaponDetector(Weapon weapon)
    {
        MeleeWeapon melee = weapon as MeleeWeapon;
        if (melee != null)
        {
            rayTable["Right" + melee.WeaponName] = new VisionCast
            (
                new Vector2(melee.AttackRange, melee.AttackWidth),
                0,
                Vector2.right,
                new Vector2(melee.AttackRange/2, 0),
                melee.HitMask,
                CastType.Box
            );
            rayTable["Left" + melee.WeaponName] = new VisionCast
            (
                new Vector2(melee.AttackRange, melee.AttackWidth),
                0,
                Vector2.left,
                new Vector2(-melee.AttackRange/2, 0),
                melee.HitMask,
                CastType.Box
            );
        }
    }
}

[Serializable]
public class VisionCast
{
    public Vector2 size;
    public float distance;
    public Vector2 direction;
    public Vector2 offset;
    public LayerMask layerMask;
    public CastType castType;
    [HideInInspector]
    public RaycastHit2D hit;

    public VisionCast(Vector2 size, float distance, Vector2 direction, Vector2 offset, LayerMask layerMask, CastType castType)
    {
        this.size = size;
        this.distance = distance;
        this.direction = direction;
        this.offset = offset;
        this.layerMask = layerMask;
        this.castType = castType;
    }
}

public enum CastType
{
    Ray,
    Box,
    Circle
}
