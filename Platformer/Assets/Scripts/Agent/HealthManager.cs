using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    public int CurrentHealth { get; private set; }

    public UnityEvent<int> OnHealthChange, OnInitializeMaxHealth;

    public void Initialize(int health)
    {
        maxHealth = health;
        OnInitializeMaxHealth?.Invoke(maxHealth);
        CurrentHealth = maxHealth;
    }

    public void AddHealth(int value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
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
