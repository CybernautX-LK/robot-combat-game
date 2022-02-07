using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Color positiveColor = Color.green;

    [SerializeField]
    private Color negativeColor = Color.red;

    [SerializeField]
    private float startValue = 100f;

    [SerializeField]
    private float minValue = 0f;

    [SerializeField]
    private float maxValue = 100f;

    [SerializeField]
    private TextMeshProUGUI textDisplay;

    [SerializeField]
    private Slider healthBar;

    public float currentValue { get; private set; }
    private Image fillImage;

    private void OnEnable()
    {
        if (healthBar != null)
            healthBar.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        if (healthBar != null)
            healthBar.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void Awake()
    {
        if (healthBar != null)
        {
            healthBar.minValue = minValue;
            healthBar.maxValue = maxValue;

            if (healthBar.fillRect != null && healthBar.fillRect.TryGetComponent(out Image image))
                fillImage = image;
        }

        SetValue(startValue);

        currentValue = 1f;
    }

    [Button]
    public void SetValue(float value)
    {
        float clampedValue = Mathf.Clamp(value, minValue, maxValue);

        if (healthBar != null)
            healthBar.SetValueWithoutNotify(clampedValue);

        currentValue = clampedValue;

        UpdateUI();
    }


    private void UpdateUI()
    {
        if (textDisplay != null)
            textDisplay.text = Mathf.RoundToInt(currentValue).ToString();

        if (fillImage != null)
            fillImage.color = Color.Lerp(negativeColor, positiveColor, currentValue / maxValue);
    }

    private void OnSliderValueChanged(float value)
    {
        SetValue(value);        
    }
}
