using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SerializableDictionaryEntry<TKey, TValue>> entries = new List<SerializableDictionaryEntry<TKey, TValue>>();

    [NonSerialized]
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize() {}

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        foreach (var entry in entries)
        {
            dictionary.Add(entry.Key, entry.Value);
        }
    }

    public TValue this[TKey key]
    {
        get 
        {
            return dictionary[key];
        }
        set 
        {
            dictionary[key] = value;
        }
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public Dictionary<TKey, TValue>.ValueCollection GetAllEntries()
    {
        return dictionary.Values;
    }
}

[Serializable]
public class SerializableDictionaryEntry<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableDictionaryEntry(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}