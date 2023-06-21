using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private GameObject current;
    [SerializeField]
    private GameObject next;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    [Range(0, 1)]
    private float speed = 0.1f;
    [SerializeField]
    [Range(0, 0.5f)]
    private float leftLimit = 0.25f;
    [SerializeField]
    [Range(0.5f, 1f)]
    private float rightLimit = 0.75f;

    private (SpriteRenderer first, float length) currentBounds;
    private (SpriteRenderer first, float length) nextBounds;
    bool isOnLeft;

    private void Awake()
    {
        playerCamera = playerCamera == null ? Camera.main : playerCamera;
    }

    private void Start()
    {
        var currentSprites = current.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.x);
        var nextSprites = next.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.x);

        currentBounds = GetBounds(currentSprites);
        nextBounds = GetBounds(nextSprites);
    }

    private (SpriteRenderer first, float length) GetBounds(IOrderedEnumerable<SpriteRenderer> sprites)
    {

        float rightX = sprites.Last().bounds.center.x + sprites.Last().bounds.size.x / 2;
        float leftX = sprites.First().bounds.center.x - sprites.First().bounds.size.x / 2;
        return (sprites.First(), rightX - leftX);
    }

    private void FixedUpdate()
    {
        float currentStartX = currentBounds.first.bounds.center.x - currentBounds.first.bounds.size.x / 2;
        float nextStartX = nextBounds.first.bounds.center.x - nextBounds.first.bounds.size.x / 2;
        float cameraPosX = playerCamera.transform.position.x;

        ShiftNext(currentStartX, cameraPosX);
        SwitchBackgrounds(nextStartX, cameraPosX);
        ApplyParallaxEffect(cameraPosX);
    }

    private void ShiftNext(float currentStartX, float cameraPosX)
    {
        if (!isOnLeft && cameraPosX < currentStartX + leftLimit * currentBounds.length)
        {
            next.transform.position -= new Vector3(currentBounds.length + nextBounds.length, 0, 0);
            isOnLeft = true;
        }
        else if (isOnLeft && cameraPosX > currentStartX + rightLimit * currentBounds.length)
        {
            next.transform.position += new Vector3(currentBounds.length + nextBounds.length, 0, 0);
            isOnLeft = false;
        }
    }

    private void SwitchBackgrounds(float nextStartX, float cameraPosX)
    {
        if (cameraPosX > nextStartX && cameraPosX < nextStartX + nextBounds.length)
        {

            var tempObject = current;
            current = next;
            next = tempObject;
            var tempBounds = currentBounds;
            currentBounds = nextBounds;
            nextBounds = tempBounds;
            isOnLeft = !isOnLeft;
        }
    }

    private void ApplyParallaxEffect(float cameraPosX)
    {
        transform.position = new Vector2(cameraPosX * speed, transform.position.y);
    }
}
