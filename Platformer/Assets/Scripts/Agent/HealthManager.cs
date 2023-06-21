using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    public int CurrentHealth { get; private set; }

    public UnityEvent OnDie;

    public UnityEvent<int> OnHealthChange;

    public UnityEvent<int> OnInitializeMaxHealth;

    public void Initialize(int health)
    {
        maxHealth = health;
        OnInitializeMaxHealth?.Invoke(maxHealth);
        CurrentHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        if (CurrentHealth <= 0)
        {
            OnDie?.Invoke();
        }
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
}
