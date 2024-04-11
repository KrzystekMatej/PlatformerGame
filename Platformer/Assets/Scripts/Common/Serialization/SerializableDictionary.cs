using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SerializableDictionaryEntry<TKey, TValue>> entries = new List<SerializableDictionaryEntry<TKey, TValue>>();

    public Dictionary<TKey, TValue> InnerTable { get; private set; } = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize() {}

    public void OnAfterDeserialize()
    {
        InnerTable = new Dictionary<TKey, TValue>();
        foreach (var entry in entries)
        {
            if (!InnerTable.ContainsKey(entry.Key))
            {
                InnerTable.Add(entry.Key, entry.Value);
            }
            else Debug.LogError($"Key {entry.Key} already exists in the dictionary.");
        }
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