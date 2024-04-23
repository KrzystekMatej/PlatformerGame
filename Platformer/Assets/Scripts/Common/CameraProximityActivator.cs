using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraProximityActivator : MonoBehaviour
{
    [SerializeField]
    private float activationDistance;
    [SerializeField]
    private float proximityCheckInterval;
    [SerializeField]
    private Transform follow;

    private Camera mainCamera;
    private bool activated = true;
    private List<GameObject> children = new List<GameObject>();
    private List<Behaviour> otherBehaviours = new List<Behaviour>();

    private void Awake()
    {
        mainCamera = Camera.main;

        for (int i = 0; i < transform.childCount; ++i)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        otherBehaviours = GetComponents<Behaviour>().Where(b => b != this).ToList();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckProximity());
    }

    private IEnumerator CheckProximity()
    {
        yield return null;

        while (true)
        {
            float cameraDistance = Vector3.Distance(follow.position, mainCamera.transform.position);
            if (activated && cameraDistance > activationDistance)
            {
                children.ForEach(c => c.SetActive(false));
                otherBehaviours.ForEach(b => b.enabled = false);
                activated = false;
            }
            else if (!activated && cameraDistance <= activationDistance)
            {
                children.ForEach(c => c.SetActive(true));
                otherBehaviours.ForEach(b => b.enabled = true);
                activated = true;
            }

            yield return new WaitForSeconds(proximityCheckInterval);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!follow) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(follow.position, activationDistance);
    }
#endif
}
