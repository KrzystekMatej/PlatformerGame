using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapons/MeleeWeapon")]
public class MeleeWeapon : AttackingWeapon
{
    public override void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask)
    {
        AttackDetector.DetectLayerMask = hitMask;
        Vector2 origin = (Vector2)attacker.bounds.center + direction * (AttackDetector.Size.x / 2);
        int colliderCount = AttackDetector.Detect(origin);


        for (int i = 0; i < colliderCount; i++)
        {
            if (AttackDetector.Colliders[i].gameObject == attacker.gameObject) continue;
            IHittable damageable = AttackDetector.Colliders[i].GetComponent<IHittable>();
            if (damageable != null) damageable.Hit(attacker, this);
        }
    }


#if UNITY_EDITOR
    public override void DrawGizmos(Vector2 origin, Vector2 direction)
    {
        AttackDetector.DrawGizmos(origin + direction * (AttackDetector.Size.x / 2));
    }
#endif
}
