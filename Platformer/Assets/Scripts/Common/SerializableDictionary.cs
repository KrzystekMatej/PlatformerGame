using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
            if (!dictionary.ContainsKey(key))
            {
                entries.Add(new SerializableDictionaryEntry<TKey, TValue>(key, value));
            }
            dictionary[key] = value;
        }
    }

    public bool Remove(TKey key)
    {
        if (dictionary.Remove(key))
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Key.Equals(key))
                {
                    entries.RemoveAt(i);
                    return true;
                }
            }
            throw new InvalidOperationException();
        }
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
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

[Serializable]
public class SoundKey
{
    public SoundActionType actionType;
    public LayerMask materialLayer;


    public SoundKey(SoundActionType actionType, LayerMask materialLayer)
    {
        this.actionType = actionType;
        this.materialLayer = materialLayer;
    }

    public override bool Equals(object obj)
    {
        if (obj is SoundKey soundKey)
        {
            return soundKey.actionType.Equals(actionType) && soundKey.materialLayer.Equals(materialLayer);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(actionType, materialLayer);
    }
}