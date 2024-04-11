#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(DieState), true)]
public class DieStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif
