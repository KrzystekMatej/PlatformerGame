using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private BackgroundData current;
    [SerializeField]
    private BackgroundData next;
    private ICinemachineCamera virtualCamera;
    [SerializeField]
    [Range(0, 1)]
    private float horizontalParallax = 0.1f;
    [SerializeField]
    [Range(0, 1)]
    private float verticalParallax = 0f;
    [SerializeField]
    [Range(0, 0.5f)]
    private float leftLimit = 0.25f;
    [SerializeField]
    [Range(0.5f, 1f)]
    private float rightLimit = 0.75f;

    Vector3 lastCameraPosition;

    private void Awake()
    {
        current.Initialize();
        next.Initialize();
        Camera.main.GetComponent<CinemachineBrain>().m_CameraActivatedEvent.AddListener(OnCameraActivated);
        enabled = false;
    }

    private void OnCameraActivated(ICinemachineCamera incomingVcam, ICinemachineCamera outgoingVcam)
    {
        virtualCamera = incomingVcam;
        lastCameraPosition = virtualCamera.State.FinalPosition;
        enabled = true;
    }

    private void FixedUpdate()
    {
        Vector3 currentCameraPosition = virtualCamera.State.FinalPosition;
        Vector2 currentLeftUp = (Vector2)current.Owner.transform.position + current.Offset;
        Vector2 nextLeftUp = (Vector2)next.Owner.transform.position + next.Offset;

        ApplyParallaxEffect(currentCameraPosition);
        ShiftNext(currentCameraPosition.x, currentLeftUp, nextLeftUp);
        SwitchBackgrounds(currentCameraPosition.x, nextLeftUp);

        lastCameraPosition = currentCameraPosition;
    }

    private void ShiftNext(float currentCameraPositionX, Vector2 currentLeftUp, Vector2 nextLeftUp)
    {
        bool isOnLeft = nextLeftUp.x < currentLeftUp.x;
        if (!isOnLeft && currentCameraPositionX < currentLeftUp.x + leftLimit * current.Size.x)
        {
            next.Owner.transform.position -= new Vector3(current.Size.x + next.Size.x, 0, 0);
        }
        else if (isOnLeft && currentCameraPositionX > currentLeftUp.x + rightLimit * current.Size.x)
        {
            next.Owner.transform.position += new Vector3(current.Size.x + next.Size.x, 0, 0);
        }
    }

    private void SwitchBackgrounds(float currentCameraPositionX, Vector2 nextLeftUp)
    {
        if (currentCameraPositionX > nextLeftUp.x && currentCameraPositionX < nextLeftUp.x + next.Size.x)
        {
            Utility.SwapReferences(ref current, ref next);
        }
    }

    private void ApplyParallaxEffect(Vector3 currentCameraPosition)
    {
        Vector3 shift = new Vector3((currentCameraPosition.x - lastCameraPosition.x) * horizontalParallax, (currentCameraPosition.y - lastCameraPosition.y) * verticalParallax);
        transform.position += shift;
    }

    public bool IsFollowed(AgentManager agent)
    {
        if (virtualCamera == null) return false;
        return agent == virtualCamera.Follow.GetComponent<AgentManager>();
    }

    private void OnDrawGizmos()
    {
        current.Initialize();
        next.Initialize();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(current.Owner.transform.position + (Vector3)current.Offset + new Vector3(current.Size.x, -current.Size.y, 0)/2, current.Size);
        Gizmos.DrawWireCube(next.Owner.transform.position + (Vector3)next.Offset + new Vector3(next.Size.x, -next.Size.y, 0) / 2, next.Size);
    }
}