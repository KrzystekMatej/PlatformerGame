using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class FastAccessList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private List<TValue> list;
    private Dictionary<TKey, TValue> dictionary;

    public IEnumerable<TValue> Values => list;
    public int Count => list.Count;

    public FastAccessList()
    {
        list = new List<TValue>();
        dictionary = new Dictionary<TKey, TValue>();
    }

    public TValue this[TKey key]
    {
        get
        {
            return dictionary[key];
        }
        set
        {
            if (dictionary.ContainsKey(key))
            {
                var index = list.IndexOf(dictionary[key]);
                list[index] = value;
                dictionary[key] = value;
            }
            else
            {
                list.Add(value);
                dictionary.Add(key, value);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        list.Add(value);
        dictionary.Add(key, value);
    }

    public bool Remove(TKey key)
    {
        if (dictionary.TryGetValue(key, out TValue value))
        {
            list.Remove(value);
            dictionary.Remove(key);
            return true;
        }

        return false;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

