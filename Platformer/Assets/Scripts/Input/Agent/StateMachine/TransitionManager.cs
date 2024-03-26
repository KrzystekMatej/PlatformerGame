using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

public class TransitionManager : GlobalComponent<TransitionManager>
{
    [HideInInspector]
    [SerializeReference]
    private List<StateTransition> availableTransitions = new List<StateTransition>();

    public List<StateTransition> AvailableTransitions => new List<StateTransition>(availableTransitions);

    public StateTransition GetTransitionByName(string name)
    {
        return availableTransitions.FirstOrDefault(t => t.GetType().Name == name);
    }
}

[CustomEditor(typeof(TransitionManager))]
public class GlobalTransitionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        TransitionManager transitionManager = (TransitionManager)target;
        SerializedProperty transitionsProperty = serializedObject.FindProperty("availableTransitions");

        if (GUILayout.Button("Reload Transition Hierarchy"))
        {
            LoadTransitionHierarchy(transitionManager, transitionsProperty, serializedObject);
        }

        DeleteNullTransitions(transitionsProperty);
        RenderAvailableTransitions(transitionManager, transitionsProperty);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed) EditorUtility.SetDirty(transitionManager);
    }

    private void DeleteNullTransitions(SerializedProperty transitionsProperty)
    {
        for (int i = 0; i < transitionsProperty.arraySize; i++)
        {
            object transition = transitionsProperty.GetArrayElementAtIndex(i).managedReferenceValue;

            if (transition == null)
            {
                transitionsProperty.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    private static void LoadTransitionHierarchyForAll()
    {
        var managers = FindObjectsOfType<TransitionManager>();
        foreach (var manager in managers)
        {
            SerializedObject serializedObject = new SerializedObject(manager);
            SerializedProperty transitionsProperty = serializedObject.FindProperty("availableTransitions");

            LoadTransitionHierarchy(manager, transitionsProperty, serializedObject);
        }
    }

    private static void LoadTransitionHierarchy(TransitionManager transitionManager, SerializedProperty transitionsProperty, SerializedObject serializedObject)
    {
        Undo.RecordObject(transitionManager, "Load Transition Hierarchy");
        List<Type> transitionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(StateTransition)))
            .ToList();

        transitionsProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();

        foreach (Type type in transitionTypes)
        {
            try
            {
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], null);
                if (type.IsSerializable && constructorInfo != null)
                {
                    transitionsProperty.arraySize++;
                    SerializedProperty newElement = transitionsProperty.GetArrayElementAtIndex(transitionsProperty.arraySize - 1);
                    newElement.managedReferenceValue = constructorInfo.Invoke(null);
                    serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    Debug.LogError($"Transition of type {type.Name} cannot be serialized");
                }
            }
            catch (MissingMethodException e)
            {
                Debug.LogError($"MissingMethodException: {e.Message}");
            }
            catch (MemberAccessException e)
            {
                Debug.LogError($"MemberAccessException: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception during instantiation: {e.Message}");
            }
        }
    }

    private void RenderAvailableTransitions(TransitionManager transitionManager, SerializedProperty transitionsProperty)
    {
        if (transitionsProperty.arraySize > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Available Transitions:");

            GUIStyle transitionStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

            for (int i = 0; i < transitionsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                object element = transitionsProperty.GetArrayElementAtIndex(i).managedReferenceValue;
                EditorGUILayout.LabelField(element.GetType().Name, transitionStyle);
                if (GUILayout.Button("Remove Transition"))
                {
                    Undo.RecordObject(transitionManager, "Remove Transition");
                    transitionsProperty.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    [DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        LoadTransitionHierarchyForAll();
    }
}


