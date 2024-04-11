#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FramerateController))]
public class FramerateControllerEditor : Editor
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