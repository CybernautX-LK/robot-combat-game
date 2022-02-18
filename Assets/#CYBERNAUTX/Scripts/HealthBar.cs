using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class HealthBar : MonoBehaviour
    {
        [BoxGroup("General")]
        [SerializeField]
        private Player player;

        [BoxGroup("Settings")]
        [SerializeField]
        private Color positiveColor = Color.green;

        [BoxGroup("Settings")]
        [SerializeField]
        private Color negativeColor = Color.red;

        [BoxGroup("Settings")]
        [SerializeField]
        private float minValue = 0f;

        [BoxGroup("Settings")]
        [SerializeField]
        private float maxValue = 100f;

        [BoxGroup("Settings")]
        [SerializeField]
        private float startValue = 100f;

        [BoxGroup("Animation")]
        [SerializeField]
        private float defaultAnimationDuration = 0.5f;

        [BoxGroup("References")]
        [SerializeField]
        private TextMeshProUGUI textDisplay;

        [BoxGroup("References")]
        [SerializeField]
        private Slider healthSlider;

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public float currentValue { get; private set; }

        private Image fillImage;

        private void Awake()
        {
            if (healthSlider != null && healthSlider.fillRect != null && healthSlider.fillRect.TryGetComponent(out Image image))
                fillImage = image;
        }

        private void Start() => Initialize();


        private void OnEnable()
        {
            if (player != null)
            {
                player.OnHealthUpdatedEvent += OnHealthUpdate;
            }
        }

        private void OnHealthUpdate(float value) => SetValue(value, defaultAnimationDuration);

        private void OnDisable()
        {
            if (player != null)
            {
                player.OnHealthUpdatedEvent -= OnHealthUpdate;
            }
        }

        public void Initialize()
        {
            if (player == null)
            {
                enabled = false;
                Debug.LogWarning($"{typeof(HealthBar).Name}: Can't initialize because player is null.");
                return;
            } 
           
            minValue = player.minHealth;
            maxValue = player.maxHealth;
            startValue = maxValue;

            if (healthSlider != null)
            {
                healthSlider.minValue = minValue;
                healthSlider.maxValue = maxValue;
            }

            SetValue(startValue);
        }

        public void SetValue(float value, float duration = 0.0f)
        {
            float newValue = Mathf.Clamp(value, minValue, maxValue);

            UpdateUI(currentValue, newValue, duration);

            currentValue = newValue;
        }


        private void UpdateUI(float fromValue, float toValue, float duration = 0.0f)
        {

            if (healthSlider != null)
            {
                healthSlider.DOValue(toValue, duration);
            }

            if (textDisplay != null)
            {
                textDisplay.DOCounter((int)fromValue, (int)toValue, duration);
            }

            if (fillImage != null)
                fillImage.color = Color.Lerp(negativeColor, positiveColor, toValue / maxValue);
        }
    }
}

