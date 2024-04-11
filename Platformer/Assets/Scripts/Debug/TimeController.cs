#if UNITY_EDITOR

using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float timeScale = 1.0f;

    void Update()
    {
        Time.timeScale = timeScale;
    }

    void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}

#endif