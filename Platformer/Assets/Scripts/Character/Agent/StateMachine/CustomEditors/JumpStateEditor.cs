#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(JumpState), true)]
public class JumpStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}


#endif