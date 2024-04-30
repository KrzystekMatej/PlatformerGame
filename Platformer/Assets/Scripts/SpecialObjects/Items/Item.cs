using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    public UnityEvent OnCollect;
    [SerializeField]
    protected Sound collectSound;

    public abstract void Collect(Collider2D collider);

    protected void PerformCollectActions(Collider2D collider)
    {
        AudioFeedback audio = collider.GetComponentInChildren<AudioFeedback>();
        if (audio) audio.PlaySpecificSound(collectSound);
        OnCollect?.Invoke();
    }


    public void OnDestroy()
    {
        if (transform.parent)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
