using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Repeater : MonoBehaviour
{
    public UnityEvent<Collider2D> OnRepeat;
    [SerializeField]
    private int repeatLimit = int.MaxValue;
    [SerializeField]
    private float repeatDelay = 1;

    private TriggerFilter triggerDetector;

    private void Awake()
    {
        triggerDetector = GetComponent<TriggerFilter>();
    }

    public void Trigger(Collider2D collider)
    {
        StartCoroutine(Repeat(collider));
    }

    private IEnumerator Repeat(Collider2D collider)
    {
        while (triggerDetector.IsColliderTriggered(collider))
        {
            OnRepeat?.Invoke(collider);
            repeatLimit--;
            if (repeatLimit == 0)
            {
                Destroy(gameObject);
                break;
            }
            yield return new WaitForSeconds(repeatDelay);
        }
    }
}
