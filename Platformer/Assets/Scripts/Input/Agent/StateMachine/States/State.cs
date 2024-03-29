using Cinemachine.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    [SerializeField]
    protected AgentManager agent;
    [HideInInspector]
    [SerializeReference]
    private List<StateTransition> orderedTransitions = new List<StateTransition>();

    public List<StateTransition> OrderedTransitions => orderedTransitions;
    public UnityEvent OnEnter, OnUpdate, OnExit;

    public void Awake()
    {
        agent = agent ? agent : GetComponentInParent<AgentManager>();
    }

    public void Start()
    {
        InitializeTransitions();
    }

    private void InitializeTransitions()
    {
        List<StateTransition> globalTransitions = new List<StateTransition>();
        for (int i = 0; i < orderedTransitions.Count; i++)
        {
            StateTransition transition = TransitionManager.Instance.GetTransitionByName(orderedTransitions[i].GetType().Name);
            if (transition != null) globalTransitions.Add(transition);
        }
        orderedTransitions.Clear();
        orderedTransitions.AddRange(globalTransitions);
    }

    public void PerformEnterActions()
    {
        HandleEnter();
        OnEnter?.Invoke();
    }

    protected abstract void HandleEnter();


    public void PerformUpdateActions()
    {
        HandleUpdate();
        OnUpdate?.Invoke();
    }

    protected abstract void HandleUpdate();
    
    public void PerformExitActions()
    {
        HandleExit();
        OnExit?.Invoke();
    }

    protected abstract void HandleExit();
}

[CustomEditor(typeof(State))]
public class StateEditor : Editor
{
    private List<StateTransition> availableTransitions;
    private int selectedIndex = 0;

    private void OnEnable()
    {
        TransitionManager transitionManager = FindObjectOfType<TransitionManager>();
        SerializedObject serializedManager = new SerializedObject(transitionManager);
        if (transitionManager != null)
        {
            availableTransitions = serializedManager
                .FindProperty("availableTransitions")
                .ArrayGetElements()
                .Select(p => p.GetValue<StateTransition>())
                .ToList();
        }
    }

    public override void OnInspectorGUI()
    {
        if (availableTransitions == null || availableTransitions.Count == 0) return;
        serializedObject.Update();

        State state = (State)target;
        SerializedProperty transitionsProperty = serializedObject.FindProperty("orderedTransitions");

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
            state.OrderedTransitions.Clear();
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
            if (transitionsProperty.arraySize == 0 || !transitionsProperty.ArrayContains<StateTransition>(t => t.GetType() == availableTransitions[selectedIndex].GetType()))
            {
                Undo.RecordObject(state, "Add Transition");
                ConstructorInfo constructorInfo = availableTransitions[selectedIndex]
                    .GetType()
                    .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
                transitionsProperty.ArrayAdd(constructorInfo.Invoke(null));
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
            EditorGUILayout.LabelField("Transition Order:");

            GUIStyle transitionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                StateTransition transition = transitionsProperty.ArrayGet<StateTransition>(i);
                EditorGUILayout.LabelField(transition.GetType().Name, transitionStyle);
                if (GUILayout.Button("Remove Transition"))
                {
                    Undo.RecordObject(state, "Remove Transition");
                    transitionsProperty.ArrayRemoveAt(i);
                    serializedObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}

