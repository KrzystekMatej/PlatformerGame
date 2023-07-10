using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private float attackDelay = 1;
    private TriggerDetector triggerDetector;


    private void Awake()
    {
        triggerDetector = GetComponent<TriggerDetector>();
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
                hittable.Hit(attackDamage);
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }
}
