using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private HealthBarUI healthBarUI;
    private PointCounterUI pointCounterUI;

    private void Awake()
    {
        healthBarUI = GetComponentInChildren<HealthBarUI>();
        pointCounterUI = GetComponentInChildren<PointCounterUI>();
    }

    public void InitializeMaxHealth(int maxHealth)
    {
        healthBarUI.Initialize(maxHealth);
    }

    public void SetHealth(int currentHealth)
    {
        healthBarUI.SetHealth(currentHealth);
    }

    public void SetPoints(int pointCount)
    {
        pointCounterUI.SetCounterValue(pointCount);
    }
}
