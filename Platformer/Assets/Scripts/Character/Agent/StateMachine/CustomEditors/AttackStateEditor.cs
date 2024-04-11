#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(AttackState), true)]
public class AttackStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif
