using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace CybernautX
{
    [CreateAssetMenu(menuName = "CybernautX/General/Player", order = 0)]
    public class Player : ScriptableObject
    {
        [BoxGroup("General")]
        public new string name = "Insert player name here...";

        [BoxGroup("Health")]
        public float maxHealth = 100f;

        [BoxGroup("Health")]
        public float minHealth = 100f;

        [BoxGroup("Health")]
        public float currentHealth;

        [BoxGroup("Weapons")]
        [InlineEditor]
        public List<ItemSlot> weaponSlots = new List<ItemSlot>();

        //[BoxGroup("Weapons")]
        //public List<Weapon> availableWeapons = new List<Weapon>();
        //
        //[BoxGroup("Weapons")]
        //public Weapon[] currentWeapons;

        [ShowInInspector]
        [ReadOnly]
        public bool isDead { get => currentHealth <= 0.0f; }

        [ShowInInspector]
        [ReadOnly]
        public int currentPoints { get; private set; }

        public UnityAction<int> OnPointsUpdatedEvent;
        public UnityAction<float> OnHealthUpdatedEvent;
        public UnityAction OnPlayerDeadEvent;

        private void OnEnable()
        {
            //currentWeapons = new Weapon[weaponSlots];
            currentHealth = maxHealth;
            currentPoints = 0;
        }

        public void Damage(float value) => SetHealth(currentHealth - value);


        public void Heal(float value) => SetHealth(currentHealth + value);

        public void Die()
        {           
            OnPlayerDeadEvent?.Invoke();
        }

        [Button]
        public void SetHealth(float value)
        {
            if (isDead) return;

            if (value <= 0.0f)
                Die();

            currentHealth = Mathf.Clamp(value, 0.0f, maxHealth);

            OnHealthUpdatedEvent?.Invoke(currentHealth);
        }

        public void SetPoints(int points)
        {
            currentPoints = points;
            OnPointsUpdatedEvent?.Invoke(points);
        } 


        //public void SetItem(Item item) { }

        //public void SetItem(Item item, int index)
        //{
        //    if (item is Weapon)
        //    {
        //        if (weaponSlots <= index) return;
        //        Weapon weapon = (Weapon)item;             
        //        currentWeapons[index] = weapon;
        //    }
        //
        //
        //}
    }
}


