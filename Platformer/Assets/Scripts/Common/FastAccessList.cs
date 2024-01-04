using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class FastAccessList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private Dictionary<TKey, TValue> dictionary;
    private List<TValue> list;

    public IEnumerable<TValue> Values => list;
    public int Count => list.Count;

    public FastAccessList()
    {
        dictionary = new Dictionary<TKey, TValue>();
        list = new List<TValue>();
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
                dictionary.Add(key, value);
                list.Add(value);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        list.Add(value);
    }

    public bool Remove(TKey key)
    {
        if (dictionary.TryGetValue(key, out TValue value))
        {
            dictionary.Remove(key);
            list.Remove(value);
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

