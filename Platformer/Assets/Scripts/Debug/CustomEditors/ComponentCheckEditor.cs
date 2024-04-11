#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComponentCheck))]
public class ComponentCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        ComponentCheck script = (ComponentCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check for Component"))
        {
            script.CheckComponent();
        }
    }
}

#endif