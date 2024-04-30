using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class BackgroundData
{
    public GameObject Owner;
    [NonSerialized]
    public Vector2 Offset;
    [NonSerialized]
    public Vector2 Size;

    public BackgroundData(GameObject owner, Vector2 offset, Vector2 size)
    {
        Owner = owner;
        Offset = offset;
        Size = size;
    }

    public void Initialize()
    {
        var spritesByX = Owner.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.x - s.bounds.size.x / 2);
        var spritesByY = Owner.GetComponentsInChildren<SpriteRenderer>().OrderBy(s => s.bounds.center.y - s.bounds.size.y / 2);

        Vector2 leftUp = new Vector2
        (
            (spritesByX.First().bounds.center.x - spritesByX.First().bounds.size.x / 2),
            (spritesByY.Last().bounds.center.y + spritesByY.Last().bounds.size.y / 2)
        );


        Offset = leftUp - (Vector2)Owner.transform.position;

        Vector2 rightDown = new Vector2
        (
            (spritesByX.Last().bounds.center.x + spritesByX.Last().bounds.size.x / 2),
            (spritesByY.First().bounds.center.y - spritesByY.First().bounds.size.y / 2)
        );

        Size = new Vector2
        (
             rightDown.x - leftUp.x,
             leftUp.y - rightDown.y
        );
    }
}
