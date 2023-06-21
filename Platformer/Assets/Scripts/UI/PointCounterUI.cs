using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointCounterUI : MonoBehaviour
{
    private TextMeshProUGUI counterText;
    public UnityEvent OnTextChange;

    private void Awake()
    {
        counterText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetCounterValue(int value)
    {
        counterText.SetText(value.ToString());
        OnTextChange?.Invoke();
    }
}
