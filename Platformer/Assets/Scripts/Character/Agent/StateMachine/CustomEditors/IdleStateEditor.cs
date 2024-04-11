#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(IdleState), true)]
public class IdleStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif