using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class State : MonoBehaviour
{
    [HideInInspector]
    [SerializeReference]
    private List<StateTransition> transitions = new List<StateTransition>();

    public List<StateTransition> Transitions => transitions;

    protected AgentManager agent;
    public UnityEvent OnEnter, OnExit;

    public void Initialize(AgentManager agent)
    {
        this.agent = agent;
        transitions = transitions
            .Select(t => GlobalTransitionManager.Instance.GetTransitionByName(t.GetType().Name))
            .ToList();
    }

    public bool AddTransition(string name)
    {
        StateTransition newTransition = GlobalTransitionManager.Instance.GetTransitionByName(name);
        if (newTransition == null) return false;
        transitions.Add(newTransition);
        return true;
    }

    public bool RemoveTransition(string name)
    {
        int removeIndex = transitions.FindIndex(t => t.GetType().Name == name);
        if (removeIndex == -1) return false;
        transitions.RemoveAt(removeIndex);
        return true;
    }

    public void Enter()
    {
        OnEnter?.Invoke();
        HandleEnter();
    }

    protected virtual void HandleEnter() { }

    public virtual void HandleUpdate() { }
    
    public void Exit()
    {
        OnExit?.Invoke();
        HandleExit();
    }

    protected virtual void HandleExit() { }
}

[CustomEditor(typeof(State))]
public class StateEditor : Editor
{
    private List<StateTransition> availableTransitions;
    private int selectedIndex = 0;

    private void OnEnable()
    {
        GlobalTransitionManager transitionManager = FindObjectOfType<GlobalTransitionManager>();
        if (transitionManager != null) availableTransitions = transitionManager.AvailableTransitions;
    }

    public override void OnInspectorGUI()
    {
        if (availableTransitions == null || availableTransitions.Count == 0) return;
        serializedObject.Update();

        State state = (State)target;
        SerializedProperty transitionsProperty = serializedObject.FindProperty("transitions");

        try
        {
            EditorGUILayout.Space();
            selectedIndex = EditorGUILayout.Popup("Select Transition", selectedIndex, availableTransitions.Select(t => t.GetType().Name).ToArray());
            EditorGUILayout.Space();
            RenderAddButton(state, transitionsProperty);
            RenderTransitions(state, transitionsProperty);

            EditorGUILayout.Space();
        }
        catch (Exception e)
        {
            Undo.RecordObject(state, "Remove Transition");
            state.Transitions.Clear();
            transitionsProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
            Debug.LogError($"Unexpected exception occurred and transitions had to be destroyed \nException message:\n{e.Message}");
        }
        if (GUI.changed) EditorUtility.SetDirty(state);
    }

    private void RenderAddButton(State state, SerializedProperty transitionsProperty)
    {
        if (GUILayout.Button("Add Transition"))
        {
            if (!state.Transitions.Any(t => t.GetType() == availableTransitions[selectedIndex].GetType()))
            {
                Undo.RecordObject(state, "Add Transition");
                transitionsProperty.arraySize++;
                SerializedProperty newElement = transitionsProperty.GetArrayElementAtIndex(transitionsProperty.arraySize - 1);
                newElement.managedReferenceValue = availableTransitions[selectedIndex];
                serializedObject.ApplyModifiedProperties();
            }
            else Debug.Log($"This state already has {availableTransitions[selectedIndex].GetType().Name}.");
        }
    }

    private void RenderTransitions(State state, SerializedProperty transitionsProperty)
    {
        if (transitionsProperty.arraySize > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Transitions:");

            GUIStyle transitionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                object transition = transitionsProperty.GetArrayElementAtIndex(i).managedReferenceValue;
                EditorGUILayout.LabelField(transition.GetType().Name, transitionStyle);
                if (GUILayout.Button("Remove Transition"))
                {
                    Undo.RecordObject(state, "Remove Transition");
                    transitionsProperty.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}

