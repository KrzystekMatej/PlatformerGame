#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SerializedPropertyExtensions
{
    public static void SetValue(this SerializedProperty property, object value)
    {
        if (value == null)
        {
            Debug.LogError("Value is null. Cannot set SerializedProperty.");
            return;
        }

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                property.intValue = (int)value;
                break;
            case SerializedPropertyType.Boolean:
                property.boolValue = (bool)value;
                break;
            case SerializedPropertyType.Float:
                property.floatValue = (float)value;
                break;
            case SerializedPropertyType.Vector2:
                property.vector2Value = (Vector2)value;
                break;
            case SerializedPropertyType.Vector3:
                property.vector3Value = (Vector3)value;
                break;
            case SerializedPropertyType.String:
                property.stringValue = (string)value;
                break;
            case SerializedPropertyType.LayerMask:
                property.intValue = (LayerMask)value;
                break;
            case SerializedPropertyType.ObjectReference:
                property.objectReferenceValue = (UnityEngine.Object)value;
                break;
            case SerializedPropertyType.ManagedReference:
            case SerializedPropertyType.Generic:
                property.managedReferenceValue = value;
                break;
            default:
                throw new ArgumentException($"Type {property.propertyType} is unsupported.");
        }
    }

    public static T GetValue<T>(this SerializedProperty property)
    {
        object value;
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                value = property.intValue;
                break;
            case SerializedPropertyType.Boolean:
                value = property.boolValue;
                break;
            case SerializedPropertyType.Float:
                value = property.floatValue;
                break;
            case SerializedPropertyType.Vector2:
                value = property.vector2Value;
                break;
            case SerializedPropertyType.Vector3:
                value = property.vector3Value;
                break;
            case SerializedPropertyType.String:
                value = property.stringValue;
                break;
            case SerializedPropertyType.LayerMask:
                value = property.intValue;
                break;
            case SerializedPropertyType.ObjectReference:
                value = property.objectReferenceValue;
                break;
            case SerializedPropertyType.ManagedReference:
            case SerializedPropertyType.Generic:
                value = property.managedReferenceValue;
                break;
            default:
                throw new ArgumentException($"Type {property.propertyType} is unsupported.");
        }

        return (T)value;
    }


    public static T ArrayGet<T>(this SerializedProperty property, int index)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        if (index < 0 || index >= property.arraySize)
        {
            throw new IndexOutOfRangeException();
        }

        return property.GetArrayElementAtIndex(index).GetValue<T>();
    }

    public static void ArraySet(this SerializedProperty property, int index, object value)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        if (index < 0 || index >= property.arraySize)
        {
            throw new IndexOutOfRangeException();
        }

        property.GetArrayElementAtIndex(index).SetValue(value);
    }

    public static void ArrayAdd(this SerializedProperty property, object value)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        property.arraySize++;
        SerializedProperty newElement = property.GetArrayElementAtIndex(property.arraySize - 1);
        newElement.SetValue(value);
    }

    public static void ArrayRemoveAt(this SerializedProperty property, int index)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        if (index < 0 || index >= property.arraySize)
        {
            throw new IndexOutOfRangeException();
        }

        SerializedProperty element = property.GetArrayElementAtIndex(index);

        if (element.propertyType == SerializedPropertyType.ObjectReference)
        {
            element.objectReferenceValue = null;
        }

        property.DeleteArrayElementAtIndex(index);
    }

    public static bool ArrayRemove<T>(this SerializedProperty property, Func<T, bool> condition)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        int containIndex = property.ArrayIndexOf(condition);

        if (containIndex >= 0)
        {
            property.ArrayRemoveAt(containIndex);
            return true;
        }
        return false;
    }


    public static int ArrayIndexOf<T>(this SerializedProperty property, Func<T, bool> condition)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        for (int i = 0; i < property.arraySize; i++)
        {
            T item = property.GetArrayElementAtIndex(i).GetValue<T>();
            if (condition(item))
            {
                return i;
            }
        }
        return -1;
    }

    public static bool ArrayContains<T>(this SerializedProperty property, Func<T, bool> condition)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        return property.ArrayIndexOf(condition) >= 0;
    }

    public static List<SerializedProperty> ArrayGetElements(this SerializedProperty property)
    {
        if (!property.isArray)
        {
            throw new ArgumentException("SerializedProperty is not an array.");
        }

        List<SerializedProperty> elements = new List<SerializedProperty>();

        for (int i = 0; i < property.arraySize; i++)
        {
            elements.Add(property.GetArrayElementAtIndex(i));
        }

        return elements;
    }
}

#endif
