#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(ClimbState), true)]
public class ClimbStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif
