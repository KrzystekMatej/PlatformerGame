#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayerMaskCheck))]
public class LayerMaskCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LayerMaskCheck script = (LayerMaskCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check Layer"))
        {
            script.CheckLayer();
        }
    }
}

#endif