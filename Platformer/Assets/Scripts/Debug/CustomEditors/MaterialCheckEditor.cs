#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialCheck))]
public class MaterialCheckEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MaterialCheck script = (MaterialCheck)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Check Materials"))
        {
            script.CheckMaterials();
        }
    }
}

#endif