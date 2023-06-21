using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIVision : MonoBehaviour
{
    [SerializeField]
    private float raycastDelay;
    [SerializeField]
    private List<VisionRay> rays = new List<VisionRay>();

    private Dictionary<string, RaycastHit2D> rayHits = new Dictionary<string, RaycastHit2D>();

    private AISteering steering;


    [Header("Gizmo parameters")]
    [SerializeField]
    Color rayColor;


    private void Awake()
    {
        steering = transform.parent.GetComponentInChildren<AISteering>();
    }

    private void Start()
    {
        StartCoroutine(CastRays());
    }

    private IEnumerator CastRays()
    {
        while (true)
        {

            foreach (VisionRay ray in rays)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + ray.offset, ray.direction, ray.length, ray.layerMask);
                rayHits[ray.name] = hit;
                ray.hit = hit;
            }
            if (steering != null) steering.ProvideRays(rays);
            yield return new WaitForSeconds(raycastDelay);
        }
    }

    public RaycastHit2D GetRaycastHit(string rayName)
    {
        return rayHits[rayName];
    }

    public void AddRay(VisionRay ray)
    {
        rays.Add(ray);
    }

    public void RemoveRay(string name)
    {
        int index = rays.FindIndex(x => x.name == name);
        if (index != -1) rays.RemoveAt(index);
        rayHits.Remove(name);
    }

    public List<VisionRay> GetAllRays()
    {
        return rays;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        foreach (VisionRay ray in rays)
        {
            Vector3 normalizedDirection = ray.direction.normalized;
            Vector3 start = (Vector2)transform.position + ray.offset;
            Vector3 end = start + new Vector3(normalizedDirection.x, normalizedDirection.y, 0) * ray.length;
            Gizmos.DrawLine(start, end);
        }
    }
}

[Serializable]
public class VisionRay
{
    public string name;
    public float length;
    public Vector2 direction;
    public Vector2 offset;
    public LayerMask layerMask;
    [HideInInspector]
    public RaycastHit2D hit;
}