#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(FallState), true)]
public class FallStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif