using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraConfinerManager : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TriggerFilter>().OnExit.AddListener(Destroy);       
    }

    public void Destroy(Collider2D objectCollider)
    {
        GameObject toDestroy = objectCollider.gameObject;
        if (!toDestroy.activeInHierarchy) return;
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
