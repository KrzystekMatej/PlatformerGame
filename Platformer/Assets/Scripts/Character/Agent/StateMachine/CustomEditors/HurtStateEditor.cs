#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(HurtState), true)]
public class HurtStateEditor : StateEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
    }
}

#endif