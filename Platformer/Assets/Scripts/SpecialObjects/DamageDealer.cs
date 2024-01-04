using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private Collider2D attacker;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private int hitLimit = int.MaxValue;
    [SerializeField]
    private float attackDelay = 1;
    private TriggerDetector triggerDetector;


    private void Awake()
    {
        triggerDetector = GetComponent<TriggerDetector>();
        attacker = GetComponent<Collider2D>();
    }

    public void Initialize(Collider2D attacker, Weapon weapon, LayerMask hitMask)
    {
        this.attacker = attacker;
        this.weapon = weapon;
        triggerDetector.ChangeTriggerMask(hitMask);
    }

    public void TriggerDamageDealer(Collider2D collider)
    {
        StartCoroutine(DealDamage(collider));
    }

    private IEnumerator DealDamage(Collider2D collider)
    {
        IHittable hittable = collider.GetComponent<IHittable>();
        if (hittable != null)
        {
            while (triggerDetector.IsColliderTriggered(collider))
            {
                hittable.Hit(attacker, weapon);
                hitLimit--;
                if (hitLimit == 0)
                {
                    Destroy(gameObject);
                }
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }
}
