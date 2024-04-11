#if UNITY_EDITOR

using UnityEngine;

public class FramerateController : MonoBehaviour
{
    [SerializeField]
    private int targetFrameRate = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void UpdateFramerate()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}

#endif
