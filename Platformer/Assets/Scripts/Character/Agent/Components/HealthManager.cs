using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [field: SerializeField]
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public UnityEvent<int> OnHealthChange, OnInitializeMaxHealth;

    public void Initialize(int health)
    {
        MaxHealth = health;
        OnInitializeMaxHealth?.Invoke(MaxHealth);
        CurrentHealth = MaxHealth;
    }

    public void AddHealth(int value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public void Kill()
    {
        AddHealth(-CurrentHealth);
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
