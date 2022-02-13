using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

namespace CybernautX
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private HealthBar playerHealthBar;

        [SerializeField]
        private HealthBar enemyHealthBar;

        [SerializeField]
        private TextMeshProUGUI ammoCounter;

        [SerializeField]
        private TextMeshProUGUI pointCounter;

        [SerializeField]
        private TextMeshProUGUI roundTimer;

        public static UnityAction<HUDController> OnAwakeEvent;

        public void Awake() => OnAwakeEvent?.Invoke(this);

        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        public void UpdateTimer(float time)
        {
            if (roundTimer == null) return;
            float clampedTime = Mathf.Clamp(time, 0.0f, Mathf.Infinity);
            roundTimer.text = GetTimeFormat(clampedTime);
        }

        private string GetTimeFormat(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);

            string formatTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            return formatTime;
        }
    }
}

