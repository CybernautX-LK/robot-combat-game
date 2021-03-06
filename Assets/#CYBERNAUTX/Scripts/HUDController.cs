using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class HUDController : MonoBehaviour
    {
        [BoxGroup("Points")]
        [SerializeField]
        private Player player;

        [BoxGroup("Points")]
        [SerializeField]
        private Player enemy;

        [BoxGroup("Points")]
        [SerializeField]
        private TextMeshProUGUI pointCounter;

        [BoxGroup("Health")]
        [SerializeField]
        private HealthBar playerHealthBar;

        [BoxGroup("Health")]
        [SerializeField]
        private HealthBar enemyHealthBar;

        [BoxGroup("Ammo")]
        [SerializeField]
        private TextMeshProUGUI ammoCounter;

        [BoxGroup("Time")]
        [SerializeField]
        private TextMeshProUGUI roundTimer;

        public static UnityAction<HUDController> OnAwakeEvent;

        public void Awake()
        {
            if (player != null)
                player.OnPointsUpdatedEvent += OnPointsUpdated;

            if (enemy != null)
                enemy.OnPointsUpdatedEvent += OnPointsUpdated;

            OnAwakeEvent?.Invoke(this);
        }

        private void Start()
        {
            UpdatePointCounter();
        }

        private void OnDestroy()
        {
            if (player != null)
                player.OnPointsUpdatedEvent -= OnPointsUpdated;

            if (enemy != null)
                enemy.OnPointsUpdatedEvent -= OnPointsUpdated;
        }

        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        public void UpdatePointCounter()
        {
            if (pointCounter == null || player == null || enemy == null) return;

            pointCounter.text = $"{player.currentPoints} : {enemy.currentPoints}";
        }

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

        private void OnPointsUpdated(int points) => UpdatePointCounter();
    }
}

