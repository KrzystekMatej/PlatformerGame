#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(FlyState), true)]
public class FlyStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif