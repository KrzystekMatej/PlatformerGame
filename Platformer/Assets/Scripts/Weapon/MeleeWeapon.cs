using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Weapons/MeleeWeapon")]
public class MeleeWeapon : AttackingWeapon
{
    public override void Attack(Collider2D attacker, Vector2 direction, LayerMask hitMask)
    {
        int detectionCount = DetectInAttackRange(attacker.bounds.center, direction, hitMask);


        for (int i = 0; i < detectionCount; i++)
        {
            if (AttackDetector.Colliders[i].gameObject == attacker.gameObject) continue;
            IHittable damageable = AttackDetector.Colliders[i].GetComponent<IHittable>();
            if (damageable != null) damageable.Hit(attacker, this);
        }
    }
}