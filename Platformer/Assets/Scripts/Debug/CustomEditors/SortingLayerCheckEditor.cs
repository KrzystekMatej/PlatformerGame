#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SortingLayerCheck))]
public class SortingLayerCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SortingLayerCheck script = (SortingLayerCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check Layer"))
        {
            script.CheckLayer();
        }
    }
}

#endif
