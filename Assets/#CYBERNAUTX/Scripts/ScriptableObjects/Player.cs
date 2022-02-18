using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public abstract class Player : ScriptableObject
    {
        public class Configuration
        {
            public bool movementEnabled;
            public bool turningEnabled;
            public bool shootingEnabled;
            public bool weaponSwitchingEnabled;

            public Configuration(bool movementEnabled = true, bool turningEnabled = true, bool shootingEnabled = true, bool weaponSwitchingEnabled = true)
            {
                this.movementEnabled = movementEnabled;
                this.turningEnabled = turningEnabled;
                this.shootingEnabled = shootingEnabled;
                this.weaponSwitchingEnabled = weaponSwitchingEnabled;
            }
        }

        public Configuration configuration = new Configuration();

        [BoxGroup("General")]
        public new string name = "Insert player name here...";

        [BoxGroup("Health")]
        public float maxHealth = 100f;

        [BoxGroup("Health")]
        public float minHealth = 0.0f;

        [BoxGroup("Health")]
        public float currentHealth;

        [BoxGroup("Weapons")]
        [InlineEditor]
        public List<ItemSlot> weaponSlots = new List<ItemSlot>();

        [BoxGroup("Settings")]
        [PropertyOrder(20f)]
        [Range(0.5f, 3.0f)]
        public float takeDamageMultiplier = 1.0f;

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public bool isDead { get => currentHealth <= 0.0f; }

        [BoxGroup("Debug")]
        [ShowInInspector]
        [ReadOnly]
        public int currentPoints { get; private set; }

        public UnityAction<int> OnPointsUpdatedEvent;
        public UnityAction<float> OnHealthUpdatedEvent;
        public UnityAction OnPlayerDeadEvent;

        private void OnEnable() => Reset();

        public void TakeDamage(float value) => SetHealth(currentHealth - value * takeDamageMultiplier);


        public void Heal(float value) => SetHealth(currentHealth + value);

        public void Die()
        {           
            OnPlayerDeadEvent?.Invoke();
        }

        public virtual void Think(RobotController controller) { }

        [Button]
        public void SetHealth(float value)
        {
            if (value <= 0.0f && !isDead)
                Die();

            currentHealth = Mathf.Clamp(value, 0.0f, maxHealth);

            OnHealthUpdatedEvent?.Invoke(currentHealth);
        }

        public void SetPoints(int points)
        {
            currentPoints = points;
            OnPointsUpdatedEvent?.Invoke(points);
        }

        public void Reset()
        {
            SetHealth(maxHealth);
            SetPoints(0);
            takeDamageMultiplier = 1.0f;
        }
    }
}


