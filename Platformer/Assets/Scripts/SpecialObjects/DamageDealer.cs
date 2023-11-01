using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private GameObject attacker;
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
        attacker = gameObject;

    }

    public void Initialize(GameObject attacker, Weapon weapon, int hitLimit)
    {
        this.attacker = attacker;
        this.weapon = weapon; 
        this.hitLimit = hitLimit;
    }

    public void TriggerDamageDealer(Collider2D collider)
    {
        DealDamage(collider);
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
