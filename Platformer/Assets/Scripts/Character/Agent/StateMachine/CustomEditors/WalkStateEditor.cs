#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(WalkState), true)]
public class WalkStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}


#endif