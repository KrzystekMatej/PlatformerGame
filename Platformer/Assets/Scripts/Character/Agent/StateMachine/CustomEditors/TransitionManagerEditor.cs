#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(TransitionManager))]
public class TransitionManagerEditor : Editor
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

        RenderAvailableTransitions(transitionManager, transitionsProperty);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed) EditorUtility.SetDirty(transitionManager);
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
                    transitionsProperty.ArrayAdd(constructorInfo.Invoke(null));
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
                StateTransition transition = transitionsProperty.ArrayGet<StateTransition>(i);
                EditorGUILayout.LabelField(transition.GetType().Name, transitionStyle);
                if (GUILayout.Button("Remove Transition"))
                {
                    Undo.RecordObject(transitionManager, "Remove Transition");
                    transitionsProperty.ArrayRemoveAt(i);
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

#endif

