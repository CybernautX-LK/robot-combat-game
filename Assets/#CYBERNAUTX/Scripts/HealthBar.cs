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

    [SerializeField]
    private float duration = 0.5f;

    public float currentValue { get; private set; }
    private Image fillImage;

    //private void OnEnable()
    //{
    //    if (healthBar != null)
    //        healthBar.onValueChanged.AddListener(OnSliderValueChanged);
    //}
    //
    //private void OnDisable()
    //{
    //    if (healthBar != null)
    //        healthBar.onValueChanged.RemoveListener(OnSliderValueChanged);
    //}

    private void Awake()
    {
        if (healthBar != null && healthBar.fillRect != null && healthBar.fillRect.TryGetComponent(out Image image))
                fillImage = image;

        SetValue(startValue);
    }

    public void Initialize(float minValue, float maxValue, float startValue)
    {
        if (healthBar != null)
        {
            healthBar.minValue = minValue;
            healthBar.maxValue = maxValue;
        }

        this.minValue = minValue;
        this.maxValue = maxValue;
        this.startValue = startValue;

        SetValue(startValue);
    }

    [Button]
    public void SetValue(float value, float duration = 0.0f)
    {
        float newValue = Mathf.Clamp(value, minValue, maxValue);
        
        UpdateUI(currentValue, newValue, duration);

        currentValue = newValue;
    }


    private void UpdateUI(float fromValue, float toValue, float duration = 0.0f)
    {

        if (healthBar != null)
        {
            healthBar.DOValue(toValue, duration);
        }

        if (textDisplay != null)
        {
            textDisplay.DOCounter((int)fromValue, (int)toValue, duration);
        }

        if (fillImage != null)
            fillImage.color = Color.Lerp(negativeColor, positiveColor, toValue / maxValue);
    }
}
