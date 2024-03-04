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
    private GameObject current;
    [SerializeField]
    private GameObject next;
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

    BackgroundData backgroundData;
    BackgroundData tempBackgroundData;

    Vector3 lastCameraPosition;

    private void Awake()
    {
        Camera.main.GetComponent<CinemachineBrain>().m_CameraActivatedEvent.AddListener(OnCameraActivated);
        enabled = false;
    }

    private void OnCameraActivated(ICinemachineCamera incomingVcam, ICinemachineCamera outgoingVcam)
    {
        virtualCamera = incomingVcam;
        var currentSprites = current.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.x);
        var nextSprites = next.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.x);

        backgroundData.CurrentBounds = GetBounds(currentSprites);
        backgroundData.NextBounds = GetBounds(nextSprites);

        tempBackgroundData = backgroundData;

        lastCameraPosition = virtualCamera.State.FinalPosition;
        enabled = true;
    }

    private (float startX, float length) GetBounds(IOrderedEnumerable<SpriteRenderer> sprites)
    {

        float rightX = sprites.Last().bounds.center.x + sprites.Last().bounds.size.x / 2;
        float leftX = sprites.First().bounds.center.x - sprites.First().bounds.size.x / 2;
        return (sprites.First().bounds.center.x - sprites.First().bounds.size.x / 2, rightX - leftX);
    }

    private void FixedUpdate()
    {
        Vector3 currentCameraPosition = virtualCamera.State.FinalPosition;

        ApplyParallaxEffect(currentCameraPosition);
        ShiftNext(currentCameraPosition.x);
        SwitchBackgrounds(currentCameraPosition.x);

        lastCameraPosition = currentCameraPosition;
    }

    private void ShiftNext(float currentCameraPositionX)
    {
        if (!backgroundData.IsOnLeft && currentCameraPositionX < backgroundData.CurrentBounds.startX + leftLimit * backgroundData.CurrentBounds.length)
        {
            Vector3 shift = new Vector3(backgroundData.CurrentBounds.length + backgroundData.NextBounds.length, 0, 0);
            next.transform.position -= shift;
            backgroundData.NextBounds.startX -= shift.x;
            backgroundData.IsOnLeft = true;
        }
        else if (backgroundData.IsOnLeft && currentCameraPositionX > backgroundData.CurrentBounds.startX + rightLimit * backgroundData.CurrentBounds.length)
        {
            Vector3 shift = new Vector3(backgroundData.CurrentBounds.length + backgroundData.NextBounds.length, 0, 0);
            next.transform.position += shift;
            backgroundData.NextBounds.startX += shift.x;
            backgroundData.IsOnLeft = false;
        }
    }

    private void SwitchBackgrounds(float currentCameraPositionX)
    {
       if (currentCameraPositionX > backgroundData.NextBounds.startX && currentCameraPositionX < backgroundData.NextBounds.startX + backgroundData.NextBounds.length)
       {
            Utility.SwapReferences(ref current, ref next);
            var tempCurrentBounds = backgroundData.CurrentBounds;
            backgroundData.CurrentBounds = backgroundData.NextBounds;
            backgroundData.NextBounds = tempCurrentBounds;
            backgroundData.IsOnLeft = !backgroundData.IsOnLeft;
       }
    }

    private void ApplyParallaxEffect(Vector3 currentCameraPosition)
    {
        Vector3 shift = new Vector3((currentCameraPosition.x - lastCameraPosition.x) * horizontalParallax, (currentCameraPosition.y - lastCameraPosition.y) * verticalParallax);
        transform.position += shift;
        backgroundData.CurrentBounds.startX += shift.x;
        backgroundData.NextBounds.startX += shift.x;
    }


    public void CacheBackgroundData()
    {
        tempBackgroundData = backgroundData;
    }

    public void RestoreBackgroundData()
    {
        backgroundData = tempBackgroundData;
    }

    public bool IsFollowed(Agent agent)
    {
        return agent == virtualCamera.Follow.GetComponent<Agent>();
    }
}
