using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
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


[CustomEditor(typeof(FramerateController))]
public class FramerateLimiterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        FramerateController script = (FramerateController)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Update Framerate"))
        {
            script.UpdateFramerate();
        }
    }
}

#endif
