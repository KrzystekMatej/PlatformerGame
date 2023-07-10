using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraConfinerManager : MonoBehaviour
{
    TriggerDetector destroyDetector;

    private void Awake()
    {
        destroyDetector = GetComponent<TriggerDetector>();
    }

    private void Start()
    {
        destroyDetector.OnExit.AddListener(Destroy);
    }

    private void Destroy(Collider2D objectCollider)
    {
        GameObject toDestroy = objectCollider.gameObject;
        Agent agent = toDestroy.GetComponent<Agent>();
        if (agent == null)
        {
            Destroy(toDestroy);
        }
        else
        {
            agent.FallOut();
        }
    }
}
