using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraConfinerManager : MonoBehaviour
{
    TriggerFilter destroyDetector;

    private void Awake()
    {
        destroyDetector = GetComponent<TriggerFilter>();
    }

    private void Start()
    {
        destroyDetector.OnExit.AddListener(Destroy);
    }

    private void Destroy(Collider2D objectCollider)
    {
        GameObject toDestroy = objectCollider.gameObject;
        AgentManager agent = toDestroy.GetComponent<AgentManager>();
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
