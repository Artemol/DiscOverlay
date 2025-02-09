using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    public double value;
    public Action SetValueCallback;
    
    [SerializeField] private CustomButton increase10Button;
    [SerializeField] private CustomButton increase1Button;
    [SerializeField] private CustomButton increasePoint1Button;
    [SerializeField] private CustomButton decreasePoint1Button;
    [SerializeField] private CustomButton decrease1Button;
    [SerializeField] private CustomButton decrease10Button;
    [SerializeField] private float step;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    public void SetValue(double v)
    {
        this.value = Mathf.Clamp((float)Math.Round(v, 3, MidpointRounding.AwayFromZero), minValue, maxValue);
        valueText.text = this.value.ToString("0.00");
        SetValueCallback?.Invoke();
    }
    
    private void Start()
    {
        increase10Button.OnClick += () => SetValue(value + 10 * step);
        increase1Button.OnClick += () => SetValue(value + step);
        increasePoint1Button.OnClick += () => SetValue(value + 0.1 * step);
        decreasePoint1Button.OnClick += () => SetValue(value - 0.1 * step);
        decrease1Button.OnClick += () => SetValue(value - step);
        decrease10Button.OnClick += () => SetValue(value - 10 * step);
        
        increase10Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{10 * step}");
        increase1Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{step:F1}");
        increasePoint1Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"+{0.1 * step:F2}");
        decreasePoint1Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{0.1 * step:F2}");
        decrease1Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{step:F1}");
        decrease10Button.GetComponentInChildren<TextMeshProUGUI>().SetText($"-{10 * step}");
    }
}
